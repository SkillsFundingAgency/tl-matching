using System;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class OnsPostcodesZipArchiveReader : IZipArchiveReader
    {
        private readonly IDataBlobUploadService _blobUploadService;
        public OnsPostcodesZipArchiveReader(IDataBlobUploadService blobUploadService)
        {
            _blobUploadService = blobUploadService;
        }

        public async Task<int> ProcessAsync(FileImportDto fileImportDto)
        {
            var outputBlobCount = 0;

            using (var zipArchive = new ZipArchive(fileImportDto.FileDataStream, ZipArchiveMode.Read))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    if (entry.FullName.StartsWith("Data/multi_csv/"))
                    {
                        Debug.WriteLine($"{entry.Name} - {entry.FullName}");
                    }

                    if (entry.FullName.StartsWith("Documents/LEP names and codes EN", StringComparison.InvariantCultureIgnoreCase) && 
                        entry.Name.EndsWith(".csv", StringComparison.InvariantCultureIgnoreCase))
                    {
                        await UploadBlobAsync(entry,
                            "localenterprisepartnership",
                            FileImportTypeExtensions.Csv,
                            fileImportDto.CreatedBy);

                        outputBlobCount++;
                    }
                }
            }

            return outputBlobCount;
        }

        private async Task UploadBlobAsync(ZipArchiveEntry entry,
            string containerName, string contentType, string createdBy)
        {
            using (var stream = entry.Open())
            {
                await _blobUploadService.UploadFromStreamAsync(
                    new DataStreamUploadDto
                    {
                        DataStream = stream,
                        ContainerName = containerName,
                        FileName = entry.Name,
                        ContentType = contentType,
                        UserName = createdBy
                    });
            }
        }
    }
}
