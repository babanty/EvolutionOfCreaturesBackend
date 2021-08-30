using EvolutionOfCreatures.App.Hubs;
using EvolutionOfCreatures.Db;
using EvolutionOfCreatures.Logic.Accounts;
using EvolutionOfCreatures.Logic.Players;
using FluentValidation;
using Infrastructure.HostExtensions.Filters;
using Infrastructure.HostExtensions.ServiceCollectionExtensions.DatabaseExtension;
using Infrastructure.Tools.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationExceptionFilter>();
                options.Filters.Add<NotFoundExceptionFilter>();
            }).AddNewtonsoftJson();


            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.MaximumReceiveMessageSize = MaximumReceiveMessageSize; 
            }).AddJsonProtocol();


            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });


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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppDomain.CurrentDomain.FriendlyName} API");
                c.RoutePrefix = string.Empty;
            });

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
