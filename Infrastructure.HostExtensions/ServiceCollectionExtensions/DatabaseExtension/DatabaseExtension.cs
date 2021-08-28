using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.HostExtensions.ServiceCollectionExtensions.DatabaseExtension
{
    public static class DatabaseExtension
    {
        /// <summary> Add Entity Framework Core DbContext </summary>
        /// <typeparam name="TContext">Child of DbContext</typeparam>
        /// <param name="configuration">According to the class AddDbExtensionOptions</param>
        public static IServiceCollection AddDb<TContext>(this IServiceCollection services, IConfiguration configuration)
                                        where TContext : DbContext
        {
            var configs = configuration.Get<DbOptions>();

            services.AddDbContext<TContext>(options =>
                                    options.UseSqlServer(configs.ConnectionString));

            if (configs.AutoDatabaseUpdate)
            {
                services.BuildServiceProvider()
                        .GetRequiredService<TContext>()
                        .Database.Migrate();
            }

            return services;
        }
    }
}
