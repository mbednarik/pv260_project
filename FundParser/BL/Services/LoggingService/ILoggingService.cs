using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundParser.BL.Services.LoggingService
{
    public interface ILoggingService
    {
        void LogInformation(string message, string source);

        void LogWarning(string message, string source);

        void LogError(string message, string source);

        void Log(string message, string source, LogLevel severity = LogLevel.None);
    }
}
