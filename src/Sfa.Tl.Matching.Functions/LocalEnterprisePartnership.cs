using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class LocalEnterprisePartnership
    {
        [FunctionName("ImportOnsPostcodes")]
        public async Task ImportOnsPostcodesAsync(
            [BlobTrigger("onspostcodes/{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject]IDataBlobUploadService blobUploadService
            //[Inject] IFileImportService<LocalEnterprisePartnershipStagingFileImportDto> fileImportService
        )
        {
            var stream = await blockBlob.OpenReadAsync(null, null, null);

            var createdByUser = blockBlob.GetCreatedByMetadata();

            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();

            //var createdRecords = await fileImportService.BulkImportAsync(new LocalEnterprisePartnershipStagingFileImportDto
            //{
            //    FileDataStream = stream,
            //    CreatedBy = blockBlob.GetCreatedByMetadata()
            //});
            //TODO: Move this to a service
            ProcessZipFile(stream, blobUploadService, createdByUser, logger);

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  //$"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }

        [FunctionName("ImportLocalEnterprisePartnership")]
        public async Task ImportLocalEnterprisePartnershipAsync(
            [BlobTrigger("localenterprisepartnership/{name}", Connection = "BlobStorageConnectionString")]
            ICloudBlob blockBlob,
            string name,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFileImportService<LocalEnterprisePartnershipStagingFileImportDto> fileImportService
        )
        {
            var stream = await blockBlob.OpenReadAsync(null, null, null);

            var createdByUser = blockBlob.GetCreatedByMetadata();

            logger.LogInformation($"Function {context.FunctionName} processing blob\n" +
                                  $"\tName:{name}\n" +
                                  $"\tSize: {stream.Length} Bytes");

            var stopwatch = Stopwatch.StartNew();
            
            var createdRecords = await fileImportService.BulkImportAsync(new LocalEnterprisePartnershipStagingFileImportDto
            {
                FileDataStream = stream,
                CreatedBy = blockBlob.GetCreatedByMetadata()
            });

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} processed blob\n" +
                                  $"\tName:{name}\n" +
                                  //$"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }

        public void ProcessZipFile(Stream stream, IDataBlobUploadService blobUploadService, string createdByUser, ILogger logger)
        {
            using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read))
            {
                //var directory = Configuration?["scriptOutputDirectory"] ?? $@"C:\temp\";
                //var filePath = Helpers.FileHelpers.GetSqlFilePath(directory, SqlFileNamePrefix);
                //Find the LEP file
                //zipArchive.GetEntry();
                
                //using (var fileStreamWriter = new StreamWriter(filePath))
                //{
                foreach (var entry in zipArchive.Entries)
                {
                    if (entry.FullName.StartsWith("Data/multi_csv/"))
                    {
                        Debug.WriteLine($"{entry.Name} - {entry.FullName}");
                    }

                    if (entry.FullName.StartsWith("Documents/LEP names and codes EN"))
                    {
                        //var lepNames = zipArchive.GetEntry("Documents/LEP names and codes EN as at 04_17 v2.csv");
                        using (var lepNamesStream = entry.Open())
                        {
                            //var reader = new BinaryReader(lepNamesStream);
                            //TODO: Add an UploadFromStreamAsync - include some metadata + container name
                            blobUploadService.UploadFromStreamAsync(
                                stream,
                                "localenterprisepartnership",
                                entry.Name,
                                FileImportTypeExtensions.Csv,
                                createdByUser);
                            //{
                            //    //Data = reader.Read()
                            //})
                            /*
                var blobContainer = await GetContainerAsync(
                    string.IsNullOrEmpty(dto.ContainerName) 
                        ? dto.ImportType.ToString().ToLowerInvariant() 
                        : dto.ContainerName);
                        */
                            lepNamesStream.Dispose();
                            //Save as "application/vnd.ms-excel"
                        }
                    }

                    //Console.WriteLine($"Processing zip file entry: {entry.Name}");
                    //using (var entryStream = entry.Open())
                    //{
                    //    //var processor = ProcessorFactory.GetProcessor(entry.Name);
                    //    //((IFileOutputProcessor)processor).FileStreamWriter = fileStreamWriter;
                    //    //processor.Process(entryStream);
                    //}
                }


                //WriteSqlEpilogue(fileStreamWriter);
                //}
            }
        }

    }
}
