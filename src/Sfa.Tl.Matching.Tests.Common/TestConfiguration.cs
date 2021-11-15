using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Tests.Common
{
    public class TestConfiguration
    {
        public static MatchingConfiguration MatchingConfiguration { get; }

        public static bool IsMockedHttpClient { get; }

        static TestConfiguration()
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

            MatchingConfiguration = ConfigurationLoader.Load(
                configuration["EnvironmentName"],
                configuration["ConfigurationStorageConnectionString"],
                configuration["Version"],
                configuration["ServiceName"]);

            IsMockedHttpClient = bool.Parse(configuration["IsMockedHttpClient"]);
        }

        public MatchingDbContext GetDbContext()
        {
            var dbOptions = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseSqlServer(MatchingConfiguration.SqlConnectionString, builder =>
                    builder
                        .EnableRetryOnFailure()
                        .UseNetTopologySuite())
                .Options;

            var matchingDbContext = new MatchingDbContext(dbOptions, false);
            return matchingDbContext;
        }

        public static string GetTestExecutionDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location!);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }
    }
}
