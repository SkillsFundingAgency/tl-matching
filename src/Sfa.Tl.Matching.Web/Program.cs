using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Web;

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddConsole()
            .AddDebug()
            .AddAzureWebAppDiagnostics()
            .AddFilter((category, level) =>
                level >= (category == "Microsoft" ? LogLevel.Error : LogLevel.Information));
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder
            .ConfigureKestrel(serverOptions =>
                serverOptions.Limits.MaxRequestBodySize = null)
            .UseStartup<Startup>();
    })
    .Build()
    .Run();
