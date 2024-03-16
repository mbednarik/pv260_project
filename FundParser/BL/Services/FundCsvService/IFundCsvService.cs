using DAL.Models;

namespace BL.Services.FundCsvService;

public interface IFundCsvService
{
    Task<bool> InsertRowIntoDb(FundCsvRow row);
    
    Task<IEnumerable<FundCsvRow>?> GetRowsFromUrl(string url);
}