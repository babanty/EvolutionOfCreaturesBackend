using Dapper;
using Infrastructure.Tools.Exceptions;
using Infrastructure.Tools.Validations;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Tools.Db
{
    public static class MsSqlDbManager
    {
        public static bool IsTableExists(string connectionString, string tableName)
        {
            CheckConnect(connectionString);
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return IsTableExistsInternal(db, tableName);
            }
        }


        /// <summary> Checks connect to Server and Db </summary>
        /// <exception cref="ConnectionException" />
        public static void CheckConnect(string connectionString)
        {
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));
            using (var connection = new SqlConnection(connectionString))
            {
                CheckConnectInternal(connection);
            }
        }


        /// <summary> If table does not exist then will create it </summary>
        public static void CheckTableExistenceAndCreate(string connectionString, string tableName, string createTableQuery)
        {
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));
            Check.NotNullOrEmpty(tableName, nameof(tableName));
            Check.NotNullOrEmpty(createTableQuery, nameof(createTableQuery));

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                CheckConnectInternal(db);

                if (!IsTableExistsInternal(db, tableName))
                    db.Execute(createTableQuery);
            }
        }


        public static bool CheckDbExistence(string connectionString)
        {
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));

            var dbName = GetDbName(connectionString);
            Check.NotNullOrEmpty(dbName, nameof(dbName));

            var connectionStringWithoutDb = GetConnectionStringWithoutDb(connectionString);

            using (var db = new SqlConnection(connectionStringWithoutDb))
            {
                var sqlScript = $"IF EXISTS(SELECT * FROM sys.databases WHERE name = '{dbName}') SELECT 1 ELSE SELECT 0";

                var result = db.Query<int>(sqlScript).FirstOrDefault();

                return result == 1;
            }
        }


        public static void CreateDb(string connectionString)
        {
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));

            var dbName = GetDbName(connectionString);
            Check.NotNullOrEmpty(dbName, nameof(dbName));

            var connectionStringWithoutDb = GetConnectionStringWithoutDb(connectionString);

            using (IDbConnection db = new SqlConnection(connectionStringWithoutDb))
            {
                var createDbQuery = $"CREATE DATABASE {dbName}";

                db.Execute(createDbQuery);
            }
        }


        public static string GetConnectionStringWithoutDb(string connectionString)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = ""
            };

            return sqlConnectionStringBuilder.ConnectionString;
        }


        public static string GetDbName(string connectionString)
        {
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            return sqlConnectionStringBuilder?.InitialCatalog;
        }


        /// <summary> Checks connect to Server and Db </summary>
        /// <exception cref="ConnectionException" />
        /// <param name="db"> using (IDbConnection db = new SqlConnection(connectionString)) </param>
        private static void CheckConnectInternal(IDbConnection db)
        {
            try
            {
                db.Open();
                db.Close();
            }
            catch (SqlException e)
            {
                throw new ConnectionException($"Connection to Sql db or Server failed. {Environment.NewLine} {e.Message}", e);
            }
        }


        private static bool IsTableExistsInternal(IDbConnection db, string tableName)
        {
            string isTableExistSqlQuery = $"IF (OBJECT_ID('{tableName}', 'U') IS NOT NULL) SELECT 1 ELSE SELECT 0";
            var isTableExistRaw = db.Query<int>(isTableExistSqlQuery);

            int isTableExist = isTableExistRaw.FirstOrDefault();
            if (isTableExist == 1) return true;

            return false;
        }
    }
}
