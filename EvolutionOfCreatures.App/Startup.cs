using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Infrastructure.Tools.Logging;
using EvolutionOfCreatures.App.Hubs;
using EvolutionOfCreatures.Db;
using Infrastructure.HostExtensions.ServiceCollectionExtensions.DatabaseExtension;
using EvolutionOfCreatures.Logic.Accounts;
using EvolutionOfCreatures.Logic.Players;
using FluentValidation;

namespace EvolutionOfCreatures.App
{
    public class Startup
    {
        private const int MaximumReceiveMessageSize = 524288; // 512KB

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.MaximumReceiveMessageSize = MaximumReceiveMessageSize; 
            }).AddJsonProtocol();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbLogger("Logging:DbLogger");

            services.AddDb<EvolutionOfCreaturesContext>(Configuration.GetSection("EvolutionOfCreaturesContext"));

            services.AddTransient<IValidator<CreateAccountRequest>, CreateAccountRequestValidator>();
            services.AddTransient<IValidator<CreatePlayerRequest>, CreatePlayerRequestValidator>();

            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IPlayerManager, PlayerManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILoggerFactory loggerFactory,
                              DbLoggerOptions dbLoggerOptions)
        {
            loggerFactory.AddDbLogger(dbLoggerOptions);
                
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SimulationSessionHub>("/simulation");
            });
        }
    }
}
