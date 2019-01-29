using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sfa.Tl.Matching.Infrastructure.Blob
{
    public interface IBlobService
    {
        Task<CloudBlockBlob> Upload(BlobData blobData);
    }
}