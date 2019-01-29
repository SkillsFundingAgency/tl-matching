using System.Threading.Tasks;
using Sfa.Tl.Matching.Infrastructure.Blob;

namespace Sfa.Tl.Matching.Application.Commands.UploadBlob
{
    public interface IUploadBlobCommand
    {
        Task Upload(BlobData blobData);
    }
}