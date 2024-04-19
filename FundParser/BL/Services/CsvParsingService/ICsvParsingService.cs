namespace FundParser.BL.Services.CsvParsingService
{
    public interface ICsvParsingService<T>
    {
        IEnumerable<T>? ParseString(string inputString, CancellationToken cancellationToken = default);
    }
}