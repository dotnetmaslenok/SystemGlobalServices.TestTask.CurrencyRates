using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SystemGlobalServices.TestTask.CurrencyRates.CurrencyRatesContracts;
using SystemGlobalServices.TestTask.CurrencyRates.Queries;
using SystemGlobalServices.TestTask.CurrencyRates.Responses;

namespace SystemGlobalServices.TestTask.CurrencyRates.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class CurrenciesRatesController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;  

    public CurrenciesRatesController(HttpClient httpClient, IMemoryCache memoryCache)
    {
        _httpClient = httpClient;
        _memoryCache = memoryCache;
    }

    [HttpGet("currencies")]
    public async Task<IActionResult> GetAllAsync([FromQuery] PaginationQuery paginationQuery)
    {
        var currentRates = await CreateOrGetCurrentRatesFromCacheAsync();

        if (currentRates is not null)
        {
            var skip = (paginationQuery.PageNumber - 1) * paginationQuery.PageSize;

            Console.WriteLine(currentRates.Date.Kind);
            return Ok(new CurrencyPaginationResponse()
            {
                Date = currentRates.Date,
                PreviousDate = currentRates.PreviousDate,
                PreviousUrl = currentRates.PreviousUrl,
                Timestamp = currentRates.Timestamp,
                Currencies = currentRates.Currencies.Skip(skip).Take(paginationQuery.PageSize).ToDictionary(k => k.Key, v => v.Value),
            });
        }

        return NoContent();
    }

    [HttpGet("currency/{currencyId}")]
    public async Task<IActionResult> GetCurrencyAsync([FromRoute] string currencyId)
    {
        var currentRates = await CreateOrGetCurrentRatesFromCacheAsync();

        if (currentRates is not null && currentRates.Currencies.TryGetValue(currencyId.ToUpperInvariant(), out var currency))
        {
            return Ok(currency);
        }
        
        return NotFound();
    }

    private async Task<CurrentRatesContract?> GetCurrentRatesAsync()
    {
        var response = await _httpClient.GetAsync(new Uri($"{CurrenciesRatesEndpoints.GetAllCurrenciesEndpoint}"));
        var content = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<CurrentRatesContract>(content);
    }

    private async Task<CurrentRatesContract?> CreateOrGetCurrentRatesFromCacheAsync()
    {
        return await _memoryCache.GetOrCreateAsync("currencies", async entry =>
        {
            var now = DateTime.Now.TimeOfDay;
            double lifetime;
            if (now > TimeSpan.FromHours(11.5))
            {
                lifetime = 24 - Math.Abs(DateTime.Now.Hour - TimeSpan.FromHours(11.5).Hours);
            }
            else
            {
                lifetime = Math.Abs(DateTime.Now.Hour - TimeSpan.FromHours(11.5).Hours);
            }
            
            entry.AbsoluteExpiration = DateTimeOffset.Now.AddHours(lifetime);
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            entry.Priority = CacheItemPriority.High;
            return await GetCurrentRatesAsync();
        });
    }
}