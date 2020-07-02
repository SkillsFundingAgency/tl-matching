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
    public class When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Empty_Db_Add : IClassFixture<LocalEnterprisePartnershipTestFixture>, IDisposable
    {
        private readonly LocalEnterprisePartnershipTestFixture _testFixture;
        private const string DataFilePath = @"LocalEnterprisePartnership\LocalEnterprisePartnership_Empty_Db_Add.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private const string Name = "LocalEnterprisePartnership_Empty_Db_Add";

        public When_LocalEnterprisePartnership_Import_File_Is_Valid_And_Empty_Db_Add(LocalEnterprisePartnershipTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
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

            var localEnterprisePartnership = _testFixture.MatchingDbContext.LocalEnterprisePartnership.FirstOrDefault(e => e.Name == Name);
            localEnterprisePartnership.Should().NotBeNull();
        }

        public void Dispose()
        {
            _testFixture.ResetData(Name);
            _testFixture?.Dispose();
        }
    }
}