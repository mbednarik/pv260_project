namespace FundParser.BL.Services.DownloaderService
{
    public interface IDownloaderService
    {
        public Task<string?> DownloadTextFileAsStringAsync(string url,
            IEnumerable<(string, string)>? headers = default, CancellationToken cancellationToken = default);
    }
}
