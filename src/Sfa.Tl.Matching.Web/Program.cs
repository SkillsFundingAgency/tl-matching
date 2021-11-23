using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Web;

try
{
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
        })
        .Build()
        .Run();
}
catch (Exception exception)
{
    Console.WriteLine(exception);
}
