using System;
using System.ComponentModel;
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

        private string GetUploadTypeDisplayName(string uploadTypeName)
        {
            var importType = Enum.Parse<DataImportType>(uploadTypeName);

            var displayNameAttribute = importType.GetCustomAttribute<DisplayNameAttribute>();
            if (displayNameAttribute != null)
            {
                return displayNameAttribute.DisplayName;
            }

            var descriptionAttribute = importType.GetCustomAttribute<DescriptionAttribute>();
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }

            return importType.Humanize();
        }
    }
}