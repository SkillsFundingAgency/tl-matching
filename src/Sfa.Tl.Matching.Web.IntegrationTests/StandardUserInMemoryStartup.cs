using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class StandardUserInMemoryStartup : InMemoryStartup
    {
        public StandardUserInMemoryStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            IsTestAdminUser = false;
            return true;
        }
    }
}