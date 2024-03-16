namespace DAL.Csv
{
    public interface ICsvDownloader<T>
    {
        Task<IEnumerable<T>?> DownloadAndParse(string url);
    }
}