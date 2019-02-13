using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_File_With_Missing_Mandatory : IClassFixture<ProviderTestFixture>
    {
        private const string DataFilePath = @"Provider\Provider-MissingMandatory.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly ProviderTestFixture _testFixture;

        public When_Provider_Imports_File_With_Missing_Mandatory(ProviderTestFixture testFixture)
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
                _createdRecordCount = await _testFixture.ProviderService.ImportProvider(new ProviderFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(ProviderTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}