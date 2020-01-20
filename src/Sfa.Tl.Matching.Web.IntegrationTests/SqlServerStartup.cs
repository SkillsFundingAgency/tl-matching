using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class SqlServerStartup : Startup
    {
        public SqlServerStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override void ConfigureConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", true)
                .Build();

            if (configuration["EnvironmentName"] == "__EnvironmentName__")
            {
                configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.local.json")
                    .Build();
            }

            MatchingConfiguration = ConfigurationLoader.Load(
                configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]);

            services.AddSingleton(MatchingConfiguration);
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            IsTestAdminUser = true;
            return true;
        }

        protected override void RegisterHttpClients(IServiceCollection services)
        {
            services.AddTransient<ILocationApiClient, DummyLocationApiClient>();
            services.AddTransient<IGoogleMapApiClient, DummyGoogleMapApiClient>();
        }
    }
}