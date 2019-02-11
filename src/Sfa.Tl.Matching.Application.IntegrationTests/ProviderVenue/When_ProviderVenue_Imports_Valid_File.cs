using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderVenue
{
    public class When_ProviderVenue_Imports_Valid_File : IClassFixture<ProviderVenueTestFixture>
    {
        private const string DataFilePath = @"ProviderVenue\ProviderVenue-Simple.xlsx";
        private int _createdRecordCount;

        private readonly string _testExecutionDirectory;

        private readonly ProviderVenueTestFixture _testFixture;
        public When_ProviderVenue_Imports_Valid_File(ProviderVenueTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.ProviderVenueService.ImportProviderVenue(new ProviderVenueFileImportDto { FileDataStream = stream });
            }

            _createdRecordCount.Should().Be(1);
        }
    }
}