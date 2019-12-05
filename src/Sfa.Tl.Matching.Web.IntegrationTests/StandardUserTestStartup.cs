using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class StandardUserTestStartup : TestStartup
    {
        public StandardUserTestStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            IsTestAdminUser = false;
            return true;
        }
    }
}