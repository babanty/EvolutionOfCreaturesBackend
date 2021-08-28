using DeepCopy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Nuget:
// Microsoft.Extensions.Configuration
// Microsoft.Extensions.DependencyInjection
// Microsoft.Extensions.Options.ConfigurationExtensions
// Microsoft.AspNetCore.Hosting.Abstractions
// DeepCloner

namespace Infrastructure.Tools.ServiceCollectionExtensions
{
    public static class SettingExtension
    {
        /// <summary> This is for getting options from appsettings without IOptions-interface </summary>
        public static IServiceCollection AddSetting<TOption>(this IServiceCollection services, string configurationSectionPath)
           where TOption : class, new()
        {
            TOption configsDeepCopy = null;

            using (var buildServiceProvider = services.BuildServiceProvider())
            {
                var configuration = buildServiceProvider.GetService<IConfiguration>();

                var configurationSection = configuration.GetSection(configurationSectionPath);
                var configs = configurationSection.Get<TOption>();

                configsDeepCopy = DeepCopier.Copy(configs);
            }

            services.AddTransient(provider =>
            {
                return configsDeepCopy;
            });

            return services;
        }


        /// <summary> register IOptions<TOption> in DI and return them </summary>
        public static TOption RegisterAndGetSetting<TOption>(this IServiceCollection services, WebHostBuilderContext context, string configurationSectionPath)
           where TOption : class, new()
        {
            IConfiguration configuration = context.Configuration.GetSection(configurationSectionPath);
            services.Configure<TOption>(configuration);
            return configuration.Get<TOption>();
        }
    }
}
