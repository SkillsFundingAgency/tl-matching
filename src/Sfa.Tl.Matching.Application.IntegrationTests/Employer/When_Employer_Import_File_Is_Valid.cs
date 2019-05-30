using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Import_File_Is_Valid : IClassFixture<EmployerTestFixture>
    {
        private readonly EmployerTestFixture _testFixture;
        private const string DataFilePath = @"Employer\Employer-Simple.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_Employer_Import_File_Is_Valid(EmployerTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData("Employer-Simple");
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.BulkImport(new EmployerStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(EmployerTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);
        }
    }
}