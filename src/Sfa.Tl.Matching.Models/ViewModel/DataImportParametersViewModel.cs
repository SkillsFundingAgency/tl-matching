using System;
using System.Linq;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class DataImportParametersViewModel
    {
        public DataImportType SelectedImportType { get; set; }
        public IFormFile File { get; set; }
        public bool IsImportSuccessful { get; set; }

        public SelectListItem[] ImportType => Enum.GetNames(typeof(DataImportType)).Select(uploadType =>
            new SelectListItem
            {

                Value = uploadType.ToString(),
                Text = ((DataImportType) Enum.Parse(typeof(DataImportType), uploadType)).Humanize()
            }).ToArray();
    }
}