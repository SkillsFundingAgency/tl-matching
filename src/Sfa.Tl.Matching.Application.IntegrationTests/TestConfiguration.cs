using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Infrastructure.Configuration;

namespace Sfa.Tl.Matching.Application.IntegrationTests
{
    [SetUpFixture]
    public class TestConfiguration
    {
        public static MatchingConfiguration MatchingConfiguration { get; private set; }

        [OneTimeSetUp]
        public async Task LoadConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            MatchingConfiguration = await ConfigurationLoader.Load(
                configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]);
        }

        public static MatchingDbContext GetDbContext()
        {
            var dbOptions = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseSqlServer(MatchingConfiguration.SqlConnectionString)
                .Options;

            var matchingDbContext = new MatchingDbContext(dbOptions);
            return matchingDbContext;
        }
    }
}
