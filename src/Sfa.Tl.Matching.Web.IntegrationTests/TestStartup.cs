using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override void ConfigureConfiguration(IServiceCollection services)
        {
            MatchingConfiguration = new MatchingConfiguration
            {

                PostcodeRetrieverBaseUrl = "https://postcodes.io",
                GoogleMapsApiBaseUrl = "https://google.com"
            };
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