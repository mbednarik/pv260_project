using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundParser.DAL.Logging
{
    public interface ILoggingService
    {
        Task LogInformation(string message, string source, CancellationToken cancellationToken = default!);

        Task LogWarning(string message, string source, CancellationToken cancellationToken = default!);

        Task LogError(string message, string source, CancellationToken cancellationToken = default!);

        Task Log(string message, string source, LogLevel severity = LogLevel.None, CancellationToken cancellationToken = default!);
    }
}
