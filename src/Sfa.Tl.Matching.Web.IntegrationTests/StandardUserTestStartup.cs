using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class StandardUserTestStartup : TestStartup
    {
        public StandardUserTestStartup(IConfiguration configuration, IWebHostEnvironment env, ILoggerFactory loggerFactory) : base(configuration, env, loggerFactory)
        {
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            IsTestAdminUser = false;
            return true;
        }
    }
}