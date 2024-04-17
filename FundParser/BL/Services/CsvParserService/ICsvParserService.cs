namespace FundParser.BL.Services.CsvParserService
{
    public interface ICsvParserService<T>
    {
        IEnumerable<T>? ParseString(string inputString, CancellationToken cancellationToken = default);
    }
}