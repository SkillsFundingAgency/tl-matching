using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Import_File_Has_Missing_Mandatory : IClassFixture<EmployerTestFixture>
    {
        private readonly EmployerTestFixture _testFixture;
        private const string DataFilePath = @"Employer\Employer-MissingMandatory.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_Employer_Import_File_Has_Missing_Mandatory(EmployerTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
        }

        [Fact]
        public async Task Then_No_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.Import(new EmployerFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(EmployerTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}