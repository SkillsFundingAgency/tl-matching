//using System.Diagnostics;
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
//            [BlobTrigger("providerqualification/{name}", Connection = "AzureWebJobsStorage")]ICloudBlob blockBlob, 
//            string name, 
//            ExecutionContext context,
//            ILogger logger,
//            IProviderQualificationService providerQualificationService
//        )
//        {
//            var stream = await blockBlob.OpenReadAsync(null, null, null);
//            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
//                                  $"\tName:{name}\n" +
//                                  $"\tSize: {stream.Length} Bytes");
//
//            var stopwatch = Stopwatch.StartNew();
//            var createdRecords = await providerQualificationService.ImportProviderQualification(new ProviderQualificationFileImportDto
//{
//                FileDataStream = stream,
//                CreatedBy = blockBlob.GetCreatedByMetadata()
//            });
//            stopwatch.Stop();
//
//            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
//                                  $"\tName:{name}\n" +
//                                  $"\tRows saved: {createdRecords}\n" +
//                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
//            logger.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {stream.Length} Bytes");
//        }
//    }
//}