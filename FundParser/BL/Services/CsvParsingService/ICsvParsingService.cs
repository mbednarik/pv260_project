namespace FundParser.BL.Services.CsvParserService
{
    public interface ICsvParsingService<T>
    {
        IEnumerable<T>? ParseString(string inputString, CancellationToken cancellationToken = default);
    }
}