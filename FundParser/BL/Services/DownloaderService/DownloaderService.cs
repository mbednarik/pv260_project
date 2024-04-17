using FundParser.BL.Services.LoggingService;

namespace FundParser.BL.Services.DownloaderService
{
    public class DownloaderService : IDownloaderService
    {
        private readonly HttpClient _client;
        private readonly ILoggingService _logger;

        public DownloaderService(ILoggingService logger) : this(logger, new HttpClientHandler())
        {
            _logger = logger;
        }

        public DownloaderService(ILoggingService logger, HttpMessageHandler messageHandler)
        {
            _client = new HttpClient(messageHandler);
            _logger = logger;
        }


        public async Task<string?> DownloadTextFileAsStringAsync(string url, IEnumerable<(string, string)>? headers = default, CancellationToken cancellationToken = default)
        {
            try
            {
                HttpRequestMessage request = new(HttpMethod.Get, new Uri(url));
                if (headers != null)
                {
                    foreach (var (key, value) in headers)
                    {
                        request.Headers.Add(key, value);
                    }
                }
                var response = await _client.SendAsync(request, cancellationToken);
                if (!response.IsSuccessStatusCode)
                {
                    await _logger.LogError($"Failed to download csv, status code: {response.StatusCode}", nameof(DownloaderService), cancellationToken);
                    return null;
                }
                using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using StreamReader reader = new(responseStream);
                return await reader.ReadToEndAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await _logger.LogError($"Unable to parse the imported csv file, error: {e.Message}", nameof(DownloaderService), cancellationToken);
                return null;
            }
        }
    }
}
