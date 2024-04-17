using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FundParser.BL.Services.LoggingService;

namespace FundParser.BL.Services.CsvParserService;

public class CsvParserService<T> : ICsvParserService<T>
{
    private readonly CsvConfiguration _csvConfig;
    private readonly ILoggingService _logger;

    public CsvParserService(ILoggingService logger)
    {
        _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            BadDataFound = null,
            IgnoreBlankLines = true
        };
        _logger = logger;
    }

    public IEnumerable<T>? ParseString(string inputString, CancellationToken cancellationToken = default)
    {
        try
        {
            using var reader = new StringReader(inputString);
            var columnCount = typeof(T).GetProperties().Length;
            _csvConfig.ShouldSkipRecord = r => r.Row.ColumnCount != columnCount;
            using var csvReader = new CsvReader(reader, _csvConfig);
            return csvReader.GetRecords<T>().ToList();
        } catch (Exception e)
        {
            _logger.LogError($"Invalid csv format: {e.Message}", nameof(CsvParserService), cancellationToken);
            return null;
        }
    }
}