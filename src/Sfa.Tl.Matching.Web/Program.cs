using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args)
                    .Build()
                    .Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddFilter((category, level) =>
                        level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.Limits.MaxRequestBodySize = null;
                        })
                        .UseStartup<Startup>();
                });
    }
}
