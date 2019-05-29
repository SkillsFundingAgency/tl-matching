using System.IO;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Imports_File_With_Invalid_Format : IClassFixture<EmployerTestFixture>
    {
        private const string DataFilePath = @"Employer\Employer-InvalidFormat.xlsx";
        private readonly int _createdRecordCount;

        public When_Employer_Imports_File_With_Invalid_Format(EmployerTestFixture testFixture)
        {
            var testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();

            var filePath = Path.Combine(testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = testFixture.FileImportService.BulkImport(new EmployerStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(EmployerTestFixture)
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Record_Is_Saved() =>
           _createdRecordCount.Should().Be(0);
    }
}