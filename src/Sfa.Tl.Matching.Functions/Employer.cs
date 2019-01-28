using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.Functions
{
    public static class Employer
    {
        [FunctionName("Employer")]
        public static void Run(
            [BlobTrigger("files/Employer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, 
            string name, 
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");




        }
    }
}