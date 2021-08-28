using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Tools.ServiceCollectionExtensions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Tools.Logging
{
    public static class ServiceCollectionExtension
    {
        /// <summary> Добавить логирование в БД. Не забудьте так же вызвать ILoggerFactory AddDbLogger в Configure </summary>
        /// <param name="configurationSectionPath"> полный путь до секции где лежат конфиги схеме DbLoggerOptions.cs </param>
        public static IServiceCollection AddDbLogger(this IServiceCollection services, string configurationSectionPath)
        {
            services.AddSetting<DbLoggerOptions>(configurationSectionPath);

            using (var buildServiceProvider = services.BuildServiceProvider())
            {
                var configuration = buildServiceProvider.GetService<DbLoggerOptions>();

                var loggerFactory= buildServiceProvider.GetService<ILoggerFactory>();

                loggerFactory.AddProvider(new DbLoggerProvider(configuration));
            }

            return services;
        }


        /// <summary> Добавить логирование в БД. Не забудьте так же вызвать IServiceCollection AddDbLogger в ConfigureServices </summary>
        public static ILoggerFactory AddDbLogger(this ILoggerFactory loggerFactory, DbLoggerOptions options)
        {
            loggerFactory.AddProvider(new DbLoggerProvider(options));
            return loggerFactory;
        }
    }
}
