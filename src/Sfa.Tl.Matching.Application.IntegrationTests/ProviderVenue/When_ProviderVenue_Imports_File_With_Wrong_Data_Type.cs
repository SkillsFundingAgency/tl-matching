using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderVenue
{
    public class When_ProviderVenue_Imports_File_With_Wrong_Data_Type : IClassFixture<ProviderVenueTestFixture>
    {
        private const string DataFilePath = @"ProviderVenue\ProviderVenue-WrongDataType.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly ProviderVenueTestFixture _testFixture;

        public When_ProviderVenue_Imports_File_With_Wrong_Data_Type(ProviderVenueTestFixture testFixture)
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
                _createdRecordCount = await _testFixture.FileImportService.Import(new ProviderVenueFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(ProviderVenueTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}