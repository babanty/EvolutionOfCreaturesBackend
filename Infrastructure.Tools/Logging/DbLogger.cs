using Dapper;
using Infrastructure.Tools.Db;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

// Nuget:
// Dapper
// Microsoft.Extensions.Logging
// System.Data.SqlClient

namespace Infrastructure.Tools.Logging
{
    public class DbLogger : ILogger
    {
        private static readonly string AppName = Assembly.GetEntryAssembly().GetName().Name;
        private const string TableName = "Logs";

        private LogLevel EnabledLogLevel { get; set; }
        private string CategoryName { get; set; }
        private string AppId { get; set; }
        private string ConnectionString { get; set; }
        private string FilePathForSosLogs { get; set; }

        private static object SosFileLocker = new object();

        public DbLogger(LogLevel EnabledLogLevel, string CategoryName, string AppId, string ConnectionString, string FilePathForSosLogs)
        {
            this.EnabledLogLevel = EnabledLogLevel;
            this.CategoryName = CategoryName;
            this.AppId = AppId;
            this.ConnectionString = ConnectionString;
            this.FilePathForSosLogs = FilePathForSosLogs;
            CheckDbAndTableExistence();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return (int)EnabledLogLevel <= (int)logLevel && logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                        Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var logMessage = new LogMessage
            {
                AppName = AppName,
                AppId = AppId,
                Level = logLevel.ToString(),
                UtcTimestamp = DateTime.UtcNow,
                Message = state.ToString(),
                Exception = exception?.GetType().FullName,
                MessageFromExeption = exception?.Message,
                StackTrace = exception?.StackTrace,
                Category = CategoryName,
                EventId = eventId.Id,
                EventName = eventId.Name,
                InnerException = exception?.InnerException?.ToString()
            };

            if (exception?.InnerException != null)
            {
                string msg = string.Empty;

                if (!string.IsNullOrEmpty(exception.InnerException.Message))
                    msg += $"Message: {exception.InnerException.Message} ";

                if (!string.IsNullOrEmpty(exception.InnerException?.ToString()))
                    msg += $"Exception: {exception.InnerException} ";

                if (!string.IsNullOrEmpty(exception.InnerException.StackTrace))
                    msg += $"StackTrace: {exception.InnerException.StackTrace} ";

                logMessage.InnerException = msg;
            }


            try
            {
                WriteLog(logMessage);
            }
            catch (Exception e)
            {
                WriteTextSos(logMessage, e);
            }
        }


        private void WriteLog(LogMessage logMessage)
        {
            string sqlQuery = @$"INSERT INTO {TableName} 
                                    (AppName, AppId, UtcTimestamp, Level, Message, MessageFromExeption, Exception, StackTrace, EventId, EventName, Category, InnerException) 
                                VALUES
                                    (@AppName, @AppId, @UtcTimestamp, @Level, @Message, @MessageFromExeption, @Exception, @StackTrace, @EventId, @EventName, @Category, @InnerException)";

            using (IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Execute(sqlQuery, logMessage);
            }
        }

        private void WriteTextSos(LogMessage logMessage, string CauseOfSos)
        {

            string msg = Environment.NewLine + "____________________________________________________________";
            msg += Environment.NewLine + $"Problem: {CauseOfSos}." + Environment.NewLine + Environment.NewLine;

            if (logMessage != null)
            {
                msg += $"{logMessage.Level} [{logMessage.UtcTimestamp}]: " +
                    $"AppName: {logMessage.AppName}. " +
                    $"AppId: {logMessage.AppId}. " +
                    $"Category: {logMessage.Category}. " +
                    $"EventId: {logMessage.EventId}. " +
                    $"EventName: {logMessage.EventName}. ";

                if (!string.IsNullOrEmpty(logMessage.Message))
                    msg += $"Message: {logMessage.Message} ";

                if (!string.IsNullOrEmpty(logMessage.Exception))
                    msg += $"Exception: {logMessage.Exception} ";

                if (!string.IsNullOrEmpty(logMessage.StackTrace))
                    msg += $"StackTrace: {logMessage.StackTrace} ";
            }

            lock (SosFileLocker)
            {
                while (true)
                {
                    if (!FileInUse(FilePathForSosLogs))
                    {
                        try { File.AppendAllText(FilePathForSosLogs, msg + Environment.NewLine); }
                        catch { continue; } // if the file is currently being created.
                        return;
                    }
                }
            }
        }


        private bool FileInUse(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return false;
            }

            var file = new FileInfo(filePath);
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }


        private void WriteTextSos(LogMessage logMessage, Exception CauseOfSos)
        {
            string causeOfSos = $"{CauseOfSos}. {CauseOfSos.StackTrace}. ";

            WriteTextSos(logMessage, causeOfSos);
        }


        /// <summary> Checks and creates table </summary>
        private void CheckDbAndTableExistence()
        {
            try
            {
                if (!MsSqlDbManager.CheckDbExistence(ConnectionString))
                    MsSqlDbManager.CreateDb(ConnectionString);

                MsSqlDbManager.CheckTableExistenceAndCreate(ConnectionString, TableName, GetCreateLogTableQuery());
            }
            catch (Exception e)
            {
                WriteTextSos(null, e);
            }
        }


        private static string GetCreateLogTableQuery()
        {
            return @$"
                        CREATE TABLE [dbo].[{TableName}](
                            	[Id] [int]  PRIMARY KEY IDENTITY,
                            	[AppName] [nvarchar](200) NOT NULL,
                            	[AppId] [nvarchar](100) NULL,
                            	[UtcTimestamp] [datetime2](7) NOT NULL,
                            	[Level] [nvarchar](20) NOT NULL,
                            	[Message] [nvarchar](max) NULL,
                            	[MessageFromExeption] [nvarchar](max) NULL,
                            	[Exception] [nvarchar](max) NULL,
                            	[StackTrace] [nvarchar](max) NULL,
                            	[EventId] [int] NULL,
                            	[EventName] [nvarchar](max) NULL,
                            	[Category] [nvarchar](max) NULL,
                                [InnerException] [nvarchar](max) NULL
                        )

                        SET ANSI_PADDING ON SET 
                        ANSI_PADDING ON
                        
                        CREATE NONCLUSTERED INDEX [_dta_index_Logs_5_1093578934__K4_K5_K10] 
                        ON [dbo].[{TableName}] ( [UtcTimestamp] ASC, [Level] ASC, [EventId] ASC)
                        WITH (SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
            ";
        }
    }
}
