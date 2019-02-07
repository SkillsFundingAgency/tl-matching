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
        [FunctionName("QualificationRoutePathMapping")]
        public static async Task ImportQualificationRoutePathMapping(
            [BlobTrigger("qualificationroutepathmapping/{name}", Connection = "BlobStorageConnectionString")]Stream stream,
            string name,
            ILogger logger,
            [Inject] IRoutePathService routePathService
        )
        {
            logger.LogInformation($"C# Blob trigger function processing blob\n Name:{name} \n Size: {stream.Length} Bytes");

            var createdRows = await routePathService.ImportQualificationPathMapping(stream);

            logger.LogInformation($"C# Blob trigger function processed blob\n Name:{name} \n Rows saved: {createdRows}");
        }
    }
}
