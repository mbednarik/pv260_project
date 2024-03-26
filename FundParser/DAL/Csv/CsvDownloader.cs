using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using FundParser.DAL.Logging;

namespace FundParser.DAL.Csv;

public class CsvDownloader<T> : ICsvDownloader<T>
{
    private const int ColumnCount = 8;
    private readonly HttpClient _client;
    private readonly ILoggingService _logger;

    public CsvDownloader(ILoggingService logger)
    {
        _logger = logger;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/csv");
        _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Content type", "text/csv");
    }

    public async Task<IEnumerable<T>?> DownloadAndParse(string url, CancellationToken cancellationToken = default)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            BadDataFound = null,
            IgnoreBlankLines = true,
            ShouldSkipRecord = r => r.Row.ColumnCount != ColumnCount
        };
        try
        {
            var response = await _client.GetAsync(new Uri(url), cancellationToken);
            var csvStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            using var reader = new StreamReader(csvStream);
            using var csvReader = new CsvReader(reader, config);

            return csvReader.GetRecords<T>().ToList();
        }
        catch (HttpRequestException e)
        {
            await _logger.LogError($"Unable to parse the imported csv file, error: {e.Message}", nameof(CsvDownloader<T>));
            return null;
        }
    }
}