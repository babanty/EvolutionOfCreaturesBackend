using Microsoft.Extensions.Logging;

namespace Infrastructure.Tools.Logging
{
    public class DbLoggerProvider : ILoggerProvider
    {

        private DbLoggerOptions DbLoggerOptions { get; set; }


        public DbLoggerProvider(DbLoggerOptions settings)
        {
            DbLoggerOptions = settings;
        }


        public ILogger CreateLogger(string categoryName)
        {

            return new DbLogger(DbLoggerOptions.LogLevel,
                                    categoryName,
                                    DbLoggerOptions.AppId,
                                    DbLoggerOptions.ConnectionString,
                                    DbLoggerOptions.FilePathForSosLogs);
        }

        public void Dispose()
        {
        }
    }
}
