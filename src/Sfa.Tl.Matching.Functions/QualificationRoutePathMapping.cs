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
    public static class QualificationRoutePathMapping
    {
        [FunctionName("ImportQualificationRoutePathMapping")]
        public static async Task ImportQualificationRoutePathMapping(
            [BlobTrigger("qualificationroutepathmapping/{name}", Connection = "BlobStorageConnectionString")]Stream stream,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IRoutePathService routePathService
        )
        {
            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            var createdRecords = await routePathService.ImportQualificationPathMapping(new QualificationRoutePathMappingFileImportDto());
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}
