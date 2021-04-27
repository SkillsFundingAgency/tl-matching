using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class PostcodeLookup
    {
        private readonly IFileImportService<PostcodeLookupStagingFileImportDto> _fileImportService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public PostcodeLookup(
            IFileImportService<PostcodeLookupStagingFileImportDto> fileImportService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _fileImportService = fileImportService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("ImportPostcodeLookup")]
        public async Task ImportPostcodeLookupAsync(
            [BlobTrigger("postcodes/{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
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

                var createdRecords = 0;

                var stopwatch = Stopwatch.StartNew();

                await using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);

                    using var zipArchive = new ZipArchive(ms, ZipArchiveMode.Read);
                    foreach (var entry in zipArchive.Entries)
                    {
                        if (zipArchive.Entries.Count == 1
                            || (entry.FullName.StartsWith("Data/ONSPD")
                                && entry.Name.StartsWith(".csv")))
                        {
                            await using var entryStream = entry.Open();
                            createdRecords = await _fileImportService.BulkImportAsync(
                                new PostcodeLookupStagingFileImportDto
                                {
                                    FileDataStream = entryStream,
                                    CreatedBy = blockBlob.GetCreatedByMetadata()
                                });

                            break;
                        }
                    }
                }

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error importing PostcodeLookup data. Internal Error Message {e}";

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
