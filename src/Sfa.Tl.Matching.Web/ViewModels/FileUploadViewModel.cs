using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Domain.Enums;

namespace Sfa.Tl.Matching.Web.ViewModels
{
    public class FileUploadViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "A file type must be selected")]
        public int SelectedFileType { get; set; }
        public List<FileTypeViewModel> FileTypeViewModels { get; set; } = new List<FileTypeViewModel>();
        public List<FileUploadType> UploadFileTypes { get; set; } = new List<FileUploadType>();
    }
}