using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class CreateEmployers
    {
        [FunctionName("CreateEmployers")]
        public static async Task Run(
            [BlobTrigger("files/Employer/{name}", Connection = "BlobStorageConnectionString")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] ICreateEmployerService createEmployerService)
        {
            logger.LogInformation($"Processing Employer blob\n Name:{name} \n Size: {stream.Length} Bytes");

            var createdRecords = await createEmployerService.Process(stream);

            logger.LogInformation($"Processed {createdRecords} Employer records from blob\n Name:{name} \n Size: {stream.Length} Bytes");
        }
    }
}