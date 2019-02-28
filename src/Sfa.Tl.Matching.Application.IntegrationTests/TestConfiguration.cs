using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Application.IntegrationTests
{
    public class TestConfiguration
    {
        public static MatchingConfiguration MatchingConfiguration { get; }

        static TestConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            MatchingConfiguration = ConfigurationLoader.Load(
                configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]);
        }

        public MatchingDbContext GetDbContext()
        {
            var dbOptions = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseSqlServer(MatchingConfiguration.SqlConnectionString, builder => builder.EnableRetryOnFailure())
                .Options;

            var matchingDbContext = new MatchingDbContext(dbOptions);
            return matchingDbContext;
        }
    }
}
