using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Infrastructure.Enums;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.ViewModels
{
    public class DataImportViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "A file type must be selected")]
        public int SelectedDataImportType { get; set; }
        public List<DataImportTypeViewModel> DataImportTypeViewModels { get; set; } = new List<DataImportTypeViewModel>();
        public List<DataImportType> UploadDataImportTypes { get; set; } = new List<DataImportType>();
        public bool Success { get; set; }
    }
}