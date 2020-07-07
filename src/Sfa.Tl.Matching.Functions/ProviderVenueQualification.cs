using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderVenueQualification
    {
        [FunctionName("ImportProviderVenueQualification")]
        public async Task ImportProviderVenueQualification(
            [BlobTrigger("providervenuequalification/{name}", Connection = "BlobStorageConnectionString")] ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IProviderVenueQualificationFileImportService fileImportService,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stream = await blockBlob.OpenReadAsync(null, null, null);

                logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tSize: {stream.Length} Bytes");

                var stopwatch = Stopwatch.StartNew();

                var updatedRecords = await fileImportService.BulkImportAsync(new ProviderVenueQualificationFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = blockBlob.GetCreatedByMetadata()
                });

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tRows updated: {updatedRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception ex)
            {
                var errormessage = $"Error importing ProviderVenueQualification data. Internal Error Message {ex}";

                logger.LogError(errormessage);
                await functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}