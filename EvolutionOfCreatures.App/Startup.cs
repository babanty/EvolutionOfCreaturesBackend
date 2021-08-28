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

namespace EvolutionOfCreatures.App
{
    public class Startup
    {
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
                hubOptions.MaximumReceiveMessageSize = 524288; // 512KB
            }).AddJsonProtocol();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddDbLogger("Logging:DbLogger");

            services.AddDb<EvolutionOfCreaturesContext>(Configuration.GetSection("EvolutionOfCreaturesContext"));
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
