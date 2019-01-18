using System.Collections.Generic;
using Sfa.Tl.Matching.Core.Enums;

namespace Sfa.Tl.Matching.Web.ViewModels
{
    public class FileUploadViewModel
    {
        public int SelectedFileType { get; set; }
        public List<FileTypeViewModel> FileTypeViewModels { get; set; } = new List<FileTypeViewModel>();
        public List<FileUploadType> UploadFileTypes { get; set; } = new List<FileUploadType>();
    }
}