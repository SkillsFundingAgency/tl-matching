using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.PostcodeLookup
{
    public class When_PostcodeLookup_Import_File_Is_Valid_And_Empty_Db_Add : IClassFixture<PostcodeLookupTestFixture>, IDisposable
    {
        private readonly PostcodeLookupTestFixture _testFixture;
        private const string DataFilePath = @"PostcodeLookup\PostcodeLookup_Empty_Db_Add.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private const string LepCode = "TESTLEP01";

        public When_PostcodeLookup_Import_File_Is_Valid_And_Empty_Db_Add(PostcodeLookupTestFixture testFixture)
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
                _createdRecordCount = await _testFixture.FileImportService.BulkImportAsync(new PostcodeLookupStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(PostcodeLookupTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);

            var postcodeLookup = _testFixture.MatchingDbContext.PostcodeLookup.FirstOrDefault(e => e.PrimaryLepCode == LepCode);
            postcodeLookup.Should().NotBeNull();
        }

        public void Dispose()
        {
            _testFixture.ResetData(LepCode);
            _testFixture?.Dispose();
        }
    }
}