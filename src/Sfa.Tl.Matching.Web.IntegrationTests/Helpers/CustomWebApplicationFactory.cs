using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.IntegrationTests.Database;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Helpers
{
    public class CustomWebApplicationFactory<TStartup>
        : CustomWebApplicationFactoryBase<TestStartup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<MatchingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("MatchingTestDb");
                    options.UseInternalServiceProvider(serviceProvider);
                });

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