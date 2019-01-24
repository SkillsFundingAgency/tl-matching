using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Infrastructure.Blob
{
    public interface IBlobService
    {
        Task Upload(BlobData blobData);
    }
}