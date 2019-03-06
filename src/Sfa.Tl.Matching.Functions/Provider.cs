using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public static class Provider
    {
        [FunctionName("ImportProvider")]
        public static async Task ImportProvider(
            [BlobTrigger("provider/{name}", Connection = "BlobStorageConnectionString")]ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider> fileImportService
        )
        {
            var stream = await blockBlob.OpenReadAsync(null, null, null);
            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            var createdRecords = await fileImportService.Import(new ProviderFileImportDto
            {
                FileDataStream = stream,
                CreatedBy = blockBlob.GetCreatedByMetadata()
            });

            var service = fileImportService as FileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>;

            logger.LogInformation($"FileImportService Logger is {service?._logger.GetType()}");
            service?._logger.LogInformation("This is a FileImportService Logger");

            var reader = service?._fileReader as ExcelFileReader<ProviderFileImportDto, ProviderDto>;

            logger.LogInformation($"FileImportService Logger is {reader?._logger.GetType()}");
            reader?._logger.LogInformation("This is a ExcelFileReader Logger");

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}