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
        public void GetFileExtensionTypeDataTests(DataImportType importType, string result)
        {
            var extensionType = importType.GetFileExtensionType();
            extensionType.Should().Be(result);
        }

        [Theory(DisplayName = "GetFileExtensionErrorMessage Data Tests")]
        [InlineData(FileImportTypeExtensions.Excel, "You must upload an Excel file with the XLSX file extension")]
        [InlineData(FileImportTypeExtensions.Csv, "You must upload a CSV file with the CSV file extension")]
        [InlineData(FileImportTypeExtensions.Zip, "You must upload a Zip file with the ZIP file extension")]
        public void GetFileExtensionErrorMessageDataTests(string fileContentType, string result)
        {
            var extensionType = fileContentType.GetFileExtensionErrorMessage();
            extensionType.Should().Be(result);
        }

        [Fact]
        public void GetFileExtensionErrorMessage_Throws_Exception_For_Unknown_File_Type()
        {
            var fileContentType = "jpeg";

            var ex = Assert.Throws<ArgumentException>(() => fileContentType.GetFileExtensionErrorMessage());

            Assert.Equal("Unexpected file content type 'jpeg'.", ex.Message);
        }
    }
}
