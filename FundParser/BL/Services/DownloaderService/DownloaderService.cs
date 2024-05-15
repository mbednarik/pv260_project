using FundParser.BL.Exceptions;
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

        public async Task<string> DownloadTextFileAsStringAsync(string url, IEnumerable<(string, string)> headers, CancellationToken cancellationToken = default)
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
                var message = $"Failed to download csv, server responded with status code: {response.StatusCode}";
                await _logger.LogError(message, nameof(DownloaderService), cancellationToken);
                throw new ApiErrorException(message);
            }
            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(responseStream);

            return await reader.ReadToEndAsync(cancellationToken);
        }
    }
}
