using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundParser.BL.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private const string LogPathSectionKey = "LogFolderPath";

        private readonly string _logFolderPath;

        public LoggingService(IConfiguration configuration)
        {
            _logFolderPath = configuration.GetRequiredSection(LogPathSectionKey).Value
                ?? throw new Exception($"{LogPathSectionKey} is not set in the configuration");
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }
        }

        public Task LogInformation(string message, string source, CancellationToken cancellationToken = default)
        {
            return Log(message, source, LogLevel.Information, cancellationToken);
        }

        public Task LogWarning(string message, string source, CancellationToken cancellationToken = default)
        {
            return Log(message, source, LogLevel.Warning, cancellationToken);
        }

        public Task LogError(string message, string source, CancellationToken cancellationToken = default)
        {
            return Log(message, source, LogLevel.Error, cancellationToken);
        }

        private async Task Log(string message, string source, LogLevel severity = LogLevel.None,
            CancellationToken cancellationToken = default)
        {
            var logFilePath = Path.Combine(_logFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");
            await File.AppendAllTextAsync(logFilePath,
                $"{DateTime.Now:HH:mm:ss} {severity}: {source}: {message}{Environment.NewLine}", cancellationToken);
        }
    }
}
