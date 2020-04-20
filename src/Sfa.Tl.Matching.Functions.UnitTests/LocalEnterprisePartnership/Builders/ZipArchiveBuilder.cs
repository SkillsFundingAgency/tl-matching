using System.IO;
using System.IO.Compression;

namespace Sfa.Tl.Matching.Functions.UnitTests.LocalEnterprisePartnership.Builders
{
    public class ZipArchiveBuilder
    {
        public Stream Build()
        {
            var archiveStream = new MemoryStream();

            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                var fileName = "Documents/LEP names and codes EN as at 04_17 v2.csv";
                var fileContent = new byte[] {1, 2};

                //foreach (var file in files)
                //{
                var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                using (var zipStream = zipArchiveEntry.Open())
                    //zipStream.Write(file.Content, 0, file.Content.Length);
                    zipStream.Write(fileContent, 0, fileContent.Length);
                //}
            }

            archiveStream.Seek(0, SeekOrigin.Begin);

            return archiveStream;
        }
    }
}