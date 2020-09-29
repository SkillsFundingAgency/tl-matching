using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.Tests.Common.Database;

namespace Sfa.Tl.Matching.Web.Tests.Common
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _instanceDatabaseName;

        public CustomWebApplicationFactory()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost");
            _instanceDatabaseName = $"MatchingTestDb-{Guid.NewGuid()}";
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder().ConfigureWebHostDefaults(builder =>
                builder.UseStartup<TStartup>());
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<MatchingDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<MatchingDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_instanceDatabaseName);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();
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
                    //throw;
                }
            });
        }
    }
}