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
    public class When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Db_Has_Existing_Record_Missing_From_Upload : IClassFixture<LocalEnterprisePartnershipTestFixture>, IDisposable
    {
        private readonly LocalEnterprisePartnershipTestFixture _testFixture;
        private const string DataFilePath = @"LocalEnterprisePartnership\LocalEnterprisePartnership_Existing_Record_Missing_From_Upload.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Db_Has_Existing_Record_Missing_From_Upload(LocalEnterprisePartnershipTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
            _testFixture.AddExisting("E00000100", "LocalEnterprisePartnership_Existing_Record_Missing_From_Upload");
            _testFixture.AddExisting("E00000099", "LocalEnterprisePartnership_Existing_Matching_Record");
        }

        [Fact]
        public async Task Then_Record_Is_Delete()
        {
            var localEnterprisePartnershipCount = _testFixture.MatchingDbContext.LocalEnterprisePartnership.Count();
            localEnterprisePartnershipCount.Should().Be(2);

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
            localEnterprisePartnershipCount.Should().Be(1);
        }

        public void Dispose()
        {
            _testFixture.ResetData();
            _testFixture?.Dispose();
        }
    }
}