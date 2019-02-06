using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class Provider
    {
        [FunctionName("ImportEmployer")]
        public static async Task ImportProvider(
            [BlobTrigger("provider/{name}", Connection = "ConfigurationStorageConnectionString")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] IProviderService providerService)
        {
            logger.LogInformation($"Processing {nameof(Provider)} blob\n Name:{name} \n Size: {stream.Length} Bytes");
            var createdRecords = await providerService.ImportProvider(stream);
            logger.LogInformation($"Processed {createdRecords} {nameof(Provider)} records from blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}
