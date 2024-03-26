using Microsoft.Extensions.Logging;

namespace FundParser.DAL.Models
{
    public class Log : BaseEntity
    {
        public string Message { get; set; }

        public LogLevel Severity { get; set; }

        public string Source { get; set; }
    }
}
