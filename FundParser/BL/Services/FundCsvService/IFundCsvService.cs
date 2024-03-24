namespace BL.Services.FundCsvService;

public interface IFundCsvService
{
    Task<int> UpdateHoldings(CancellationToken cancellationToken = default);
}