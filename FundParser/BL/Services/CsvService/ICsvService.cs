using DAL.Models;

namespace BL.Services.CsvService;

public interface ICsvService
{
    Task<bool> InsertRowIntoDb(CsvRow row);
    
    Task<IEnumerable<CsvRow>?> GetRowsFromUrl(string url);
}