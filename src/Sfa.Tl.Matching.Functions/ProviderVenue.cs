//using System.Diagnostics;
//using System.IO;
//using AutoMapper;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Sfa.Tl.Matching.Functions.Extensions;

//namespace Sfa.Tl.Matching.Functions
//{
//    public static class ProviderVenue
//    {
//        [FunctionName("ImportProviderVenue")]
//        public static void ImportProviderVenue(
//            [BlobTrigger("providervenue/{name}", Connection = "AzureWebJobsStorage")]Stream stream, 
//            string name, 
//            ExecutionContext context,
//            ILogger logger,
//            [Inject] IMapper mapper
//        )
//        {
//            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
//                                  $"\tName:{name}\n" +
//                                  $"\tSize: {stream.Length} Bytes");
//
//            var stopwatch = Stopwatch.StartNew();
//            var createdRecords = await providerVenueService.ImportProviderVenue(stream);
//            stopwatch.Stop();
//
//            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
//                                  $"\tName:{name}\n" +
//                                  $"\tRows saved: {createdRecords}\n" +
//                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
//            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
//            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
//        }
//    }
//}