using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class FileImportTypeExtensions
    {
        public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Csv = "application/vnd.ms-excel";

        public static string GetFileExtensionType(this DataImportType importFileType)
        {
            var memInfo = importFileType.GetType().GetTypeInfo().GetDeclaredField(importFileType.ToString());

            var attribute = memInfo?.GetCustomAttributes(true).Single(attributeType => attributeType is FileExtensionsAttribute);

            if (attribute != null)
                return typeof(FileExtensionsAttribute)
                .GetProperty("Extensions")
                ?.GetValue(attribute)
                .ToString();

            return null;
        }

        public static string GetFileExtensionErrorMessage(this string fileContentType)
        {
            var isExcel = fileContentType == Excel;
            var fileExtensionType = isExcel ? "XLSX" : "CSV";
            var fileType = isExcel ? "Excel" : "CSV";
            
            return $"You must upload an {fileType} file with the {fileExtensionType} file extension";
        }
    }
}