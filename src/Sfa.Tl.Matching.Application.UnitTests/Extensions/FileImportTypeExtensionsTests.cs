using System;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    public class FileImportTypeExtensionsTests
    {
        [Theory(DisplayName = "GetFileExtensionType Data Tests")]
        [InlineData(DataImportType.LearningAimReference, FileImportTypeExtensions.Csv)]
        [InlineData(DataImportType.LocalEnterprisePartnership, FileImportTypeExtensions.Csv)]
        [InlineData(DataImportType.Postcodes, FileImportTypeExtensions.Zip)]
        [InlineData(DataImportType.ProviderVenueQualification, FileImportTypeExtensions.Excel)]
        public void GetFileExtensionTypeDataTests(DataImportType importType, string result)
        {
            var extensionType = importType.GetFileExtensionType();
            extensionType.Should().Be(result);
        }

        [Theory(DisplayName = "GetFileExtensionErrorMessage Data Tests")]
        [InlineData(FileImportTypeExtensions.Excel, 
            FileImportTypeExtensions.Csv, 
            "You must upload an Excel file with the XLSX file extension. " +
            "Expected content type application/vnd.openxmlformats-officedocument.spreadsheetml.sheet but found application/vnd.ms-excel.")]
        [InlineData(FileImportTypeExtensions.Csv, 
            FileImportTypeExtensions.Excel, 
            "You must upload a CSV file with the CSV file extension. " +
            "Expected content type application/vnd.ms-excel but found application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.")]
        [InlineData(FileImportTypeExtensions.Zip, 
            FileImportTypeExtensions.Csv, 
            "You must upload a Zip file with the ZIP file extension. " +
            "Expected content type application/x-zip-compressed but found application/vnd.ms-excel.")]
        public void GetFileExtensionErrorMessageDataTests(string expectedFileContentType, string actualFileContentType, string result)
        {
            var extensionType = expectedFileContentType.GetFileExtensionErrorMessage(actualFileContentType);
            extensionType.Should().Be(result);
        }

        [Fact]
        public void GetFileExtensionErrorMessage_Throws_Exception_For_Unknown_File_Type()
        {
            const string fileContentType = "jpeg";
            const string expectedfileContentType = FileImportTypeExtensions.Csv;

            var ex = Assert.Throws<ArgumentException>(() 
                => fileContentType.GetFileExtensionErrorMessage(expectedfileContentType));

            Assert.Equal("Unexpected file content type 'jpeg'.", ex.Message);
        }
    }
}
