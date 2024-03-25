using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using Microsoft.Extensions.Logging;

namespace FundParser.BL.Services.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly IRepository<Log> _logRepository;

        public LoggingService(IRepository<Log> logRepository)
        {
            _logRepository = logRepository;
        }

        public void LogInformation(string message, string source)
        {
            _logRepository.InsertAsync(new Log { Message = message, Source = source, Severity = LogLevel.Information });
        }

        public void LogWarning(string message, string source)
        {
            _logRepository.InsertAsync(new Log { Message = message, Source = source, Severity = LogLevel.Warning });
        }

        public void LogError(string message, string source)
        {
            _logRepository.InsertAsync(new Log { Message = message, Source = source, Severity = LogLevel.Error });
        }

        public void Log(string message, string source, LogLevel severity = LogLevel.None)
        {
            _logRepository.InsertAsync(new Log { Message = message, Source = source, Severity = severity });
        }
    }
}
