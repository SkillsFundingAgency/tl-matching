using System.IO;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class Provider
    {
        [FunctionName("Provider")]
        public static void ImportProvider(
            [BlobTrigger("provider/{name}", Connection = "BlobStorageConnectionString")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] IMapper mapper
            )
        {
            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}
