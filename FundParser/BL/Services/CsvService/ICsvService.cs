using DAL.Models;

namespace BL.Services.CsvService;

public interface ICsvService
{
    Task<bool> InsertRowIntoDb(FundCsvRow row);
    
    Task<IEnumerable<FundCsvRow>?> GetRowsFromUrl(string url);
}