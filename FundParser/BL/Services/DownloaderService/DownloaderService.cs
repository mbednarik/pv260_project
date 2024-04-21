using FundParser.BL.Services.LoggingService;

namespace FundParser.BL.Services.DownloaderService
{
    public class DownloaderService : IDownloaderService
    {
        private readonly HttpClient _client;
        private readonly ILoggingService _logger;

        public DownloaderService(ILoggingService logger, HttpClient httpClient)
        {
            _client = httpClient;
            _logger = logger;
        }

        public async Task<string?> DownloadTextFileAsStringAsync(string url, IEnumerable<(string, string)>? headers = default, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url));
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
                    await _logger.LogError($"Failed to download csv, server responded with status code: {response.StatusCode}",
                        nameof(DownloaderService), cancellationToken);
                    return null;
                }
                using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                using var reader = new StreamReader(responseStream);

                return await reader.ReadToEndAsync(cancellationToken);
            }
            catch (Exception e)
            {
                await _logger.LogError($"Unable to download file from {url}, error: {e.Message}",
                    nameof(DownloaderService), cancellationToken);
                return null;
            }
        }
    }
}
