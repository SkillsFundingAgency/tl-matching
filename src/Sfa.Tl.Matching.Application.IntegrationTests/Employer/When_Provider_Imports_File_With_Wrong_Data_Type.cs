using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Import_File_Has_Wrong_Data_Type : IClassFixture<EmployerTestFixture>
    {
        private const string DataFilePath = @"Employer\Employer-WrongDataType.xlsx";
        private int _createdRecordCount;
        private readonly EmployerTestFixture _testFixture;
        private readonly string _testExecutionDirectory;

        public When_Employer_Import_File_Has_Wrong_Data_Type(EmployerTestFixture testFixture)
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
                _createdRecordCount = await _testFixture.EmployerService.ImportEmployer(new EmployerFileImportDto { FileDataStream = stream });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}