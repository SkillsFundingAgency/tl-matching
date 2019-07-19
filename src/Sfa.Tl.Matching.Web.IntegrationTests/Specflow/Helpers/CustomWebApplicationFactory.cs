using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Specflow.Helpers
{
    public class CustomWebApplicationFactory<TStartup> 
        : WebApplicationFactory<TStartup> where TStartup: class
    {

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder(null)
                .UseApplicationInsights()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddFilter((category, level) =>
                        level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));

                })
                .UseUrls("https://localhost")
                .UseStartup<TStartup>();
        }
    }
}
