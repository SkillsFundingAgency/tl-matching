//using System.IO;
//using AutoMapper;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;
//using Sfa.Tl.Matching.Functions.Extensions;

//namespace Sfa.Tl.Matching.Functions
//{
//    public static class ProviderQualification
//    {
//        [FunctionName("ImportProviderQualification")]
//        public static void ImportProviderQualification(
//            [BlobTrigger("providerqualification/{name}", Connection = "AzureWebJobsStorage")]Stream stream, 
//            string name, 
//            ILogger logger,
//            [Inject] IMapper mapper
//        )
//        {
//            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
//        }
//    }
//}