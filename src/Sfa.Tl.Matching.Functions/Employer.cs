using System.IO;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class Employer
    {
        [FunctionName("ImportEmployer")]
        public static void ImportEmployer(
            [BlobTrigger("employer/{name}", Connection = "AzureWebJobsStorage")]Stream stream, 
            string name, 
            ILogger logger,
            [Inject] IMapper mapper,
            [Inject] IEmployerService employerService
            )
        {
            logger.LogInformation($"Processing Employer blob\n Name:{name} \n Size: {stream.Length} Bytes");

            employerService.ImportEmployer();
        }
    }
}