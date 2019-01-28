using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Infrastructure.Blob;

namespace Sfa.Tl.Matching.Application.Commands.UploadBlob
{
    public class UploadBlobCommand : IUploadBlobCommand
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<UploadBlobCommand> _logger;

        public UploadBlobCommand(ILogger<UploadBlobCommand> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        public async Task Upload(BlobData blobData)
        {
            _logger.LogInformation("Uploading blob");
            await _blobService.Upload(blobData);
        }
    }
}