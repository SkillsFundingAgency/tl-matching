using System.IO;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class QualificationRoutePathMapping
    {
        [FunctionName("QualificationRoutePathMapping")]
        public static void ImportQualificationRoutePathMapping(
            [BlobTrigger("routeandpathway/{name}", Connection = "BlobStorageConnectionString")]Stream stream,
            string name,
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject] IRoutePathService routePathService
        )
        {
            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");

            routePathService.ImportQualificationPathMapping(stream);
        }
    }
}
