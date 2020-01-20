using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.Tests.Common.Database;

namespace Sfa.Tl.Matching.Web.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup> : CustomWebApplicationFactoryBase<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddApplicationInsightsTelemetry();

                if (typeof(TStartup).Name == "SqlServerStartup")
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.test.json", true)
                        .Build();

                    if (configuration["EnvironmentName"] == "__EnvironmentName__")
                    {
                        configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.local.json")
                            .Build();
                    }

                    var matchingConfiguration = ConfigurationLoader.Load(
                        configuration["EnvironmentName"],
                        configuration["ConfigurationStorageConnectionString"],
                        configuration["Version"],
                        configuration["ServiceName"]);

                    services.AddEntityFrameworkSqlServer();

                    services.AddDbContext<MatchingDbContext>(options =>
                        options
                            .UseSqlServer(matchingConfiguration.SqlConnectionString,
                                optionsBuilder => optionsBuilder
                                    .UseNetTopologySuite()
                                    .EnableRetryOnFailure()), ServiceLifetime.Transient);

                }
                else
                {


                    services.AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<MatchingDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("MatchingTestDb");
                        //options.UseInternalServiceProvider(serviceProvider);
                    });
                }

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MatchingDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureCreated();

                    try
                    {
                        StandingDataLoad.Load(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            $"test database. Error: {ex.Message}");
                    }
                }
            });
        }
    }
}