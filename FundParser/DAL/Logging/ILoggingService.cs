using Microsoft.Extensions.Logging;

namespace FundParser.DAL.Logging
{
    public interface ILoggingService
    {
        Task LogInformation(string message, string source, CancellationToken cancellationToken = default);

        Task LogWarning(string message, string source, CancellationToken cancellationToken = default);

        Task LogError(string message, string source, CancellationToken cancellationToken = default);

        Task Log(string message, string source, LogLevel severity = LogLevel.None, CancellationToken cancellationToken = default);
    }
}
