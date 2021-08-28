using System;

namespace Infrastructure.Tools.Logging
{
    public class LogMessage
    {
        public string AppName { get; set; }

        public string AppId { get; set; }

        public DateTime UtcTimestamp { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string MessageFromExeption { get; set; }

        public string Exception { get; set; }

        public string StackTrace { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        /// <summary> This is usually namespace </summary>
        public string Category { get; set; }

        public string InnerException { get; set; }
    }
}
