using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Functions.UnitTests.PostcodeLookup.Builders
{
    public class ZipArchiveBuilder
    {
        public Stream Build()
        {
            var archiveStream = new MemoryStream();

            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                WriteZipArchiveEntry(archive,
                    "Data/ONSPD_MAY_2019_UK.csv",
                    new byte[] { 1, 2, 3, 4 })
                    .GetAwaiter().GetResult();
            }

            archiveStream.Seek(0, SeekOrigin.Begin);

            return archiveStream;
        }

        private async Task WriteZipArchiveEntry(ZipArchive archive, string fileName, byte[] content)
        {
            var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
            await using var zipStream = zipArchiveEntry.Open();
            await zipStream.WriteAsync(content, 0, content.Length);
        }
    }
}
