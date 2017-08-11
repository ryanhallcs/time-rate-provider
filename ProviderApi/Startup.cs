using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProviderApi.Models;
using ProviderApi.Services;
using ProviderApi.Core;
using Microsoft.AspNetCore.Mvc;

namespace ProviderApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            // Load static sample data into configuration
            var sampleProjectText = File.ReadAllText(env.ContentRootFileProvider.GetFileInfo("./data/sample.json").PhysicalPath);
            builder.AddInMemoryCollection(new Dictionary<string, string> { {"sampleData", sampleProjectText} });

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options => options.AddMetricsResourceFilter());
		
            // Add App Metrics for /ping /health and /metrics endpoints
            services.AddMetrics()
                    .AddJsonSerialization()
                    .AddHealthChecks()
                    .AddMetricsMiddleware();

            // Add hardcoded sample
            services.Add(new ServiceDescriptor(
                typeof(Dictionary<Guid, RateGroup>), 
                provider =>
                    new Dictionary<Guid, RateGroup> { {Guid.Empty, RateGroupLoader.LoadFromJsonString(Configuration["sampleData"])} }, 
                ServiceLifetime.Singleton));
            
            // Rate services
            services.AddSingleton<IGroupProvider, InMemoryGroupProvider>();
            services.AddTransient<IGroupRateService, GroupRateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMetrics();
            app.UseMvc();
        }
    }
}
