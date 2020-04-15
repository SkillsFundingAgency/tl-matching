using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureKestrel(webBuilder =>
                {
                    webBuilder.Limits.MaxRequestBodySize = null;
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddFilter((category, level) =>
                        level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));

                });
    }
}
