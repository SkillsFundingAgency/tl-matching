//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Sfa.Tl.Matching.Data;
//using Sfa.Tl.Matching.Models.Configuration;

//namespace Sfa.Tl.Matching.Web.Tests.Common
//{
//    public class TestConfiguration
//    {
//        public static MatchingConfiguration MatchingConfiguration { get; }
//        public static string ApplicationUrl;

//        static TestConfiguration()
//        {
//            var configuration = new ConfigurationBuilder()
//                .AddJsonFile("appsettings.test.json")
//                .Build();

//            MatchingConfiguration = ConfigurationLoader.Load(
//                configuration["EnvironmentName"],
//                configuration["ConfigurationStorageConnectionString"],
//                configuration["Version"],
//                configuration["ServiceName"]);

//            ApplicationUrl = configuration["ApplicationUrl"];
//        }

//        public MatchingDbContext GetDbContext()
//        {
//            var dbOptions = new DbContextOptionsBuilder<MatchingDbContext>()
//                .UseSqlServer(MatchingConfiguration.SqlConnectionString, builder =>
//                    builder
//                        .EnableRetryOnFailure()
//                        .UseNetTopologySuite())
//                .Options;

//            var matchingDbContext = new MatchingDbContext(dbOptions);
//            return matchingDbContext;
//        }

//        public static string GetTestExecutionDirectory()
//        {
//            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
//            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
//            return Path.GetDirectoryName(codeBasePath);
//        }
//    }
//}