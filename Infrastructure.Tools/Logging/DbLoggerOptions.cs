using Microsoft.Extensions.Logging;

namespace Infrastructure.Tools.Logging
{
    public class DbLoggerOptions
    {
        public LogLevel LogLevel { get; set; }

        public string AppId { get; set; }

        public string ConnectionString { get; set; }

        public string FilePathForSosLogs { get; set; }
    }
}
