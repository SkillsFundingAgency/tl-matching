//using System.Diagnostics;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public static class ProviderVenue
    {
        [FunctionName("ImportProviderVenue")]
        public static async Task ImportProviderVenue(
            [BlobTrigger("providervenue/{name}", Connection = "BlobStorageConnectionString")]Stream stream,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject]IProviderVenueService providerVenueService
        )
        {
            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            var createdRecords = 0;
            createdRecords = await providerVenueService.ImportProviderVenue(new ProviderVenueFileImportDto { FileDataStream = stream });
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}