using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class QualificationRoutePathMapping
    {
        [FunctionName("ImportQualificationRoutePathMapping")]
        public static async Task ImportQualificationRoutePathMapping(
            [BlobTrigger("qualificationroutepathmapping/{name}", Connection = "BlobStorageConnectionString")]Stream stream,
            string name,
            ILogger logger,
            [Inject] IRoutePathService routePathService,
            ExecutionContext context
        )
        {
            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            var createdRows = await routePathService.ImportQualificationPathMapping(stream);
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tRows saved: {createdRows}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}
