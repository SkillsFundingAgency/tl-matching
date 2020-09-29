using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LocalEnterprisePartnership
{
    public class When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Db_Has_Existing_Non_Matching_Record : IClassFixture<LocalEnterprisePartnershipTestFixture>, IDisposable
    {
        private readonly LocalEnterprisePartnershipTestFixture _testFixture;
        private const string DataFilePath = @"LocalEnterprisePartnership\LocalEnterprisePartnership_Existing_Non_Matching_Record.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Db_Has_Existing_Non_Matching_Record(LocalEnterprisePartnershipTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
            _testFixture.AddExisting("E00000301", "Existing-LocalEnterprisePartnership");
        }

        [Fact]
        public async Task Then_New_Record_Is_Saved()
        {
            var localEnterprisePartnershipCount = _testFixture.MatchingDbContext.LocalEnterprisePartnership.Count();
            localEnterprisePartnershipCount.Should().Be(1);

            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            await using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.BulkImportAsync(new LocalEnterprisePartnershipStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(LocalEnterprisePartnershipTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);

            localEnterprisePartnershipCount = _testFixture.MatchingDbContext.LocalEnterprisePartnership.Count();
            localEnterprisePartnershipCount.Should().Be(2);
        }

        public void Dispose()
        {
            _testFixture.ResetData();
            _testFixture?.Dispose();
        }
    }
}