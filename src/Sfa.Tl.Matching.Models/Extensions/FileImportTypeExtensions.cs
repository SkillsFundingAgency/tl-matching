using System.Linq;
using System.Reflection;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Extensions
{
    public static class FileImportTypeExtensions
    {
        public const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Csv = "Text/csv";

        public static (string, string) GetFileExtensionTypeAndErrorMessage(this DataImportType importFileType)
        {
            var memInfo = importFileType.GetType().GetTypeInfo().GetDeclaredField(importFileType.ToString());

            var attribute = memInfo.GetCustomAttributes(true).Single(atype => atype.GetType().FullName == "System.ComponentModel.DataAnnotations.FileExtensionsAttribute");

            var extensionsPropInfo = attribute.GetType().GetProperties().Single(prop => prop.Name == "Extensions");
            var errorMessagePropInfo = attribute.GetType().GetProperties().Single(prop => prop.Name == "ErrorMessage");

            var extension = extensionsPropInfo.GetValue(attribute).ToString();
            var errorType = errorMessagePropInfo.GetValue(attribute).ToString();
            var fileExtensionType = errorType == Excel ? "XLSX" : "CSV";

            var errorMessage = $"You must upload an {errorType} file with the {fileExtensionType} file extension";

            return (extension, errorMessage);
        }
    }
}