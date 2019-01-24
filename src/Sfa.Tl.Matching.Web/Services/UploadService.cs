using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Application.Commands.UploadBlob;
using Sfa.Tl.Matching.Core.Enums;
using Sfa.Tl.Matching.Infrastructure.Blob;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Services
{
    public class UploadService : IUploadService
    {
        private readonly IUploadBlobCommand _uploadBlobCommand;

        public UploadService(IUploadBlobCommand uploadBlobCommand)
        {
            _uploadBlobCommand = uploadBlobCommand;
        }

        public async Task Upload(IFormFile file, FileUploadViewModel viewModel)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var blobData = new BlobData(file.FileName, 
                    (FileUploadType)viewModel.SelectedFileType,
                    file.ContentType,
                    fileBytes);

                await _uploadBlobCommand.Upload(blobData);
            }
        }
    }
}