using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_Valid_File : IClassFixture<ProviderTestFixture>
    {
        private const string DataFilePath = @"Provider\Provider-Simple.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        private readonly ProviderTestFixture _testFixture;
        public When_Provider_Imports_Valid_File(ProviderTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData("Provider-Simple");
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.ProviderService.ImportProvider(new ProviderFileImportDto { FileDataStream = stream });
            }

            _createdRecordCount.Should().Be(1);
        }
    }
}