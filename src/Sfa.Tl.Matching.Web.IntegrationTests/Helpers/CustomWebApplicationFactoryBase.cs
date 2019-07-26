using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Helpers
{
    public abstract class CustomWebApplicationFactoryBase<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseApplicationInsights()
                .UseStartup<TStartup>();
    }
}