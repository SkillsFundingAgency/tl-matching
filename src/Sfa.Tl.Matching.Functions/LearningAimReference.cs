using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class LearningAimReference
    {
        private readonly IFileImportService<LearningAimReferenceStagingFileImportDto> _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public LearningAimReference(
            IFileImportService<LearningAimReferenceStagingFileImportDto> fileImportService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _fileImportService = fileImportService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("ImportLearningAimReference")]
        public async Task ImportLearningAimReferenceAsync(
            [BlobTrigger("learningaimreference/{name}", Connection = "BlobStorageConnectionString")] ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var stream = await blockBlob.OpenReadAsync(null, null, null);

                logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tSize: {stream.Length} Bytes");

                var stopwatch = Stopwatch.StartNew();

                var createdRecords = await _fileImportService.BulkImportAsync(new LearningAimReferenceStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = blockBlob.GetCreatedByMetadata()
                });

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error importing LearningAimReference data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}