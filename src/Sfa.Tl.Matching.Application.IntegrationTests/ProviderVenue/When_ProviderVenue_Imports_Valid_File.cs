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
        private readonly Domain.Models.Provider _createdProvider;
        private const int UkPrn = 10000546;

        public When_ProviderVenue_Imports_Valid_File(ProviderVenueTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData(UkPrn);
            _createdProvider = _testFixture.CreateProvider(UkPrn);
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.Import(
                    new ProviderVenueFileImportDto
                    {
                        FileDataStream = stream,
                        ProviderId = _createdProvider.Id,
                        Source = _createdProvider.Source,
                        CreatedBy = nameof(ProviderVenueTestFixture)
                    });
            }

            _createdRecordCount.Should().Be(1);
        }
    }
}