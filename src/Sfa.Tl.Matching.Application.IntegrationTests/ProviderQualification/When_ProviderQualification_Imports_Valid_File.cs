using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderQualification
{
    public class When_ProviderQualification_Imports_Valid_File : IClassFixture<ProviderQualificationTestFixture>
    {
        private const string DataFilePath = @"ProviderQualification\ProviderQualification-Simple.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly ProviderQualificationTestFixture _testFixture;
        private readonly Domain.Models.Provider _createdProvider;
        private const int UkPrn = 10000006;
        private const string Postcode = "CV1 2WT";
        private const string LarsId = "12345678";

        public When_ProviderQualification_Imports_Valid_File(ProviderQualificationTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
            _createdProvider = _testFixture.CreateVenueAndQualification(UkPrn, Postcode, LarsId);
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.Import(
                    new ProviderQualificationFileImportDto
                    {
                        FileDataStream = stream,
                        ProviderVenueId = _createdProvider.Id,
                        Source = _createdProvider.Source,
                        CreatedBy = nameof(ProviderQualificationTestFixture)
                    });
            }

            _createdRecordCount.Should().Be(1);
        }
    }
}