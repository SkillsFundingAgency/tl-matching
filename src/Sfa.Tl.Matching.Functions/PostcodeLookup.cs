using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
    public class PostcodeLookup
    {
        [FunctionName("ImportPostcodeLookup")]
        public async Task ImportPostcodeLookupAsync(
            [BlobTrigger("postcodes/{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFileImportService<PostcodeLookupStagingFileImportDto> fileImportService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                var stream = await blockBlob.OpenReadAsync(null, null, null);

                logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                      $"\tName:{name}\n" +
                                      $"\tSize: {stream.Length} Bytes");

                var createdRecords = 0;

                var stopwatch = Stopwatch.StartNew();

                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);

                    using (var zipArchive = new ZipArchive(ms, ZipArchiveMode.Read))
                    {
                        foreach (var entry in zipArchive.Entries)
                        {
                            if (zipArchive.Entries.Count == 1
                                || (entry.FullName.StartsWith("Data/ONSPD")
                                    && entry.Name.StartsWith(".csv")))
                            {
                                using (var entryStream = entry.Open())
                                {
                                    createdRecords = await fileImportService.BulkImportAsync(
                                        new PostcodeLookupStagingFileImportDto
                                        {
                                            FileDataStream = entryStream,
                                            CreatedBy = blockBlob.GetCreatedByMetadata()
                                        });

                                    break;
                                }
                            }
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
                var errormessage = $"Error importing PostcodeLookup data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}
