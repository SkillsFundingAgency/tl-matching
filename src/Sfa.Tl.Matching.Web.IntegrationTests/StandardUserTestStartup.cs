using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class StandardUserTestStartup : TestStartup
    {
        public StandardUserTestStartup(IConfiguration configuration, IWebHostEnvironment env) : base(configuration, env)
        {
        }

        protected override bool ConfigurationIsLocalOrDev()
        {
            IsTestAdminUser = false;
            return true;
        }
    }
}