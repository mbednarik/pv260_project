using FundParser.DAL.Models;
using FundParser.DAL.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FundParser.DAL.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly IRepository<Log> _logRepository;
        private readonly FundParserDbContext _context;

        public LoggingService(IRepository<Log> logRepository,
             FundParserDbContext isolatedContext) 
        {
            _logRepository = logRepository;
            _context = isolatedContext;
        }

        public async Task LogInformation(string message, string source, CancellationToken cancellationToken = default!)
        {
            await Log(message, source, LogLevel.Information, cancellationToken);
        }

        public async Task LogWarning(string message, string source, CancellationToken cancellationToken = default!)
        {
            await Log(message, source, LogLevel.Warning, cancellationToken);
        }

        public async Task LogError(string message, string source, CancellationToken cancellationToken = default!)
        {
            await Log(message, source, LogLevel.Error, cancellationToken);
        }

        public async Task Log(string message, string source, LogLevel severity = LogLevel.None, CancellationToken cancellationToken = default!)
        {
            await _logRepository.Insert(new Log { Message = message, Source = source, Severity = severity }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
