using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;
using Infrastructure.Tools.Db;
using System.Data;
using Dapper;

namespace EvolutionOfCreatures.Logic.Tests.TestTools
{
    internal static class DbContextUtilities
    {
        public static TDbContext GetEfCoreInMemoryDb<TDbContext>() where TDbContext : DbContext => 
                                            GetDbContextInstance(GetEfCoreInMemoryDbOptions<TDbContext>());


        public static TDbContext GetSqlLiteInMemoryDb<TDbContext>() where TDbContext : DbContext =>
                                            GetDbContextInstance(GetSqlLiteInMemoryDbOptions<TDbContext>(), true);

        public static TDbContext GetMsSqlTestDb<TDbContext>() where TDbContext : DbContext =>
                                            GetDbContextInstance(GetMsSqlTestDbOptions<TDbContext>(), true);


        public static void DisposeAndDeleteMsSqlDb<TDbContext>(this TDbContext dbContext) where TDbContext : DbContext
        {
            var connectionString = dbContext.Database.GetConnectionString();

            dbContext.Dispose();

            MsSqlDbManager.DeleteDb(connectionString);
        }


        private static TDbContext GetDbContextInstance<TDbContext>(DbContextOptions<TDbContext> dbContextOptions, bool withMigrations = false) 
                 where TDbContext : DbContext
        {
            var context = Activator.CreateInstance(typeof(TDbContext), dbContextOptions) as TDbContext;

            if(withMigrations)
                context.Database.Migrate();

            return context;
        }


        private static DbContextOptions<TDbContext> GetMsSqlTestDbOptions<TDbContext>() where TDbContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();
            var connectionString = $"Server=localhost;Database={nameof(TDbContext)}_Test;Trusted_Connection=True;MultipleActiveResultSets=true;Connect Timeout=15;Encrypt=False;Packet Size=4096";

            builder.UseSqlServer(connectionString);
            return builder.Options;
        }


        private static DbContextOptions<TDbContext> GetEfCoreInMemoryDbOptions<TDbContext>() where TDbContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();
            builder.UseInMemoryDatabase(nameof(TDbContext));
            return builder.Options;
        }


        private static DbContextOptions<TDbContext> GetSqlLiteInMemoryDbOptions<TDbContext>() where TDbContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TDbContext>();

            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            builder.UseSqlite(connection);

            return builder.Options;
        }
    }
}
