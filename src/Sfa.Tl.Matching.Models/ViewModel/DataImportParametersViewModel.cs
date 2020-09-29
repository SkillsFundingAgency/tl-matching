using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Extensions;

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
                Text = GetUploadTypeDisplayName(uploadType)
            }).ToArray();

        private static string GetUploadTypeDisplayName(string uploadTypeName)
        {
            var importType = Enum.Parse<DataImportType>(uploadTypeName);

            var displayAttribute = importType.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                if (!string.IsNullOrWhiteSpace(displayAttribute.Name))
                    return displayAttribute.Name;
                if (!string.IsNullOrWhiteSpace(displayAttribute.Description))
                    return displayAttribute.Description;
            }

            return importType.Humanize();
        }
    }
}