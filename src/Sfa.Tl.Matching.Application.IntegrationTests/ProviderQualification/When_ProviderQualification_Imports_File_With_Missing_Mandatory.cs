using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderQualification
{
    public class When_ProviderQualification_Imports_File_With_Missing_Mandatory : IClassFixture<ProviderQualificationTestFixture>
    {
        private const string DataFilePath = @"ProviderQualification\ProviderQualification-MissingMandatory.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly ProviderQualificationTestFixture _testFixture;

        public When_ProviderQualification_Imports_File_With_Missing_Mandatory(ProviderQualificationTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
        }

        [Fact]
        public async Task Then_No_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.Import(new ProviderQualificationFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(ProviderQualificationTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}