namespace FundParser.BL.Services.DownloaderService
{
    public interface IDownloaderService
    {
        Task<string?> DownloadTextFileAsStringAsync(string url,
            IEnumerable<(string, string)>? headers = default, CancellationToken cancellationToken = default);
    }
}
