using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override void ConfigureConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json", true)
               .Build();

            if (configuration["EnvironmentName"] == "__EnvironmentName__")
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.local.json")
                    .Build();

            MatchingConfiguration = ConfigurationLoader.Load(
                configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]);
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            return true;
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}