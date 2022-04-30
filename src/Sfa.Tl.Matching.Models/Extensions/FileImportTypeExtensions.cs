using System;
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
        public const string CsvText = "text/csv";
        public const string AllCsv = $"{Csv},{CsvText}";
        public const string Zip = "application/x-zip-compressed";

        public static string GetFileExtensionType(this DataImportType importFileType)
        {
            var memInfo = importFileType.GetType().GetTypeInfo().GetDeclaredField(importFileType.ToString());

            var attribute = memInfo?.GetCustomAttributes(true).Single(attributeType => attributeType is FileExtensionsAttribute);

            if (attribute != null)
                return typeof(FileExtensionsAttribute)
                .GetProperty("Extensions")
                ?.GetValue(attribute)
                ?.ToString();

            return null;
        }

        public static string GetFileExtensionErrorMessage(this string fileContentType, string foundFileContentType)
        {
            var (fileExtensionType, fileType, article) = fileContentType switch
            {
                Csv or CsvText or AllCsv => ("CSV", "CSV", "a"),
                Excel => ("XLSX", "Excel", "an"),
                Zip => ("ZIP", "Zip", "a"),
                _ => throw new ArgumentException($"Unexpected file content type '{fileContentType}'.")
            };

            return $"You must upload {article} {fileType} file with the {fileExtensionType} file extension. " +
                   $"Expected content type {fileContentType.Replace(",", " or ")} " +
                   $"but found {foundFileContentType}.";
        }
    }
}