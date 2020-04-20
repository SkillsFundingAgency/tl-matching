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
    public class When_LocalEnterprisePartnership_Import_File_Is_Invalid : IClassFixture<LocalEnterprisePartnershipTestFixture>, IDisposable
    {
        private readonly LocalEnterprisePartnershipTestFixture _testFixture;
        private const string DataFilePath = @"LocalEnterprisePartnership\LocalEnterprisePartnership_Invalid.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_LocalEnterprisePartnership_Import_File_Is_Invalid(LocalEnterprisePartnershipTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
        }

        [Fact]
        public async Task Then_Record_Is_Not_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.BulkImportAsync(new LocalEnterprisePartnershipStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(LocalEnterprisePartnershipTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);

            var localEnterprisePartnership = _testFixture.MatchingDbContext.LocalEnterprisePartnership.Count(e => e.Name == "LocalEnterprisePartnership_Invalid");
            localEnterprisePartnership.Should().Be(0);
        }

        public void Dispose()
        {
            _testFixture?.Dispose();
        }
    }
}