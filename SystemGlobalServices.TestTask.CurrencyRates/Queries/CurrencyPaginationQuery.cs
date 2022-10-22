namespace SystemGlobalServices.TestTask.CurrencyRates.Queries;

public sealed class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100;
}