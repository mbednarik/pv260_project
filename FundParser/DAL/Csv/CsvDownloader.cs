using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace DAL.Csv;

public class CsvDownloader<T> : ICsvDownloader<T>
{
    private readonly HttpClient _client;

    public CsvDownloader()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/csv");
        _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
            "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
        _client.DefaultRequestHeaders.TryAddWithoutValidation("Content type", "text/csv");
    }

    public async Task<IEnumerable<T>?> DownloadAndParse(string url)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            BadDataFound = null,
            IgnoreBlankLines = true,
            ShouldSkipRecord = r => r.Row.ColumnCount != 8
        };
        try
        {
            HttpResponseMessage response = await _client.GetAsync(new Uri(url));
            var csvStream = await response.Content.ReadAsStreamAsync();

            using var reader = new StreamReader(csvStream);
            using var csvReader = new CsvReader(reader, config);

            return csvReader.GetRecords<T>().ToList();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("Error: {0} ", e.Message);
            return null;
        }
    }
}