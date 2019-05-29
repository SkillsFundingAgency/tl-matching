using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimsReference
{
    public class When_LearningAimsReference_Import_File_Is_Valid_And_Db_Has_Existing_Matching_Record : IClassFixture<LearningAimsReferenceTestFixture>, IDisposable
    {
        private readonly LearningAimsReferenceTestFixture _testFixture;
        private const string DataFilePath = @"LearningAimsReference\LearningAimsReference_Existing_Matching_Record.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_LearningAimsReference_Import_File_Is_Valid_And_Db_Has_Existing_Matching_Record(LearningAimsReferenceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
            _testFixture.AddExisting("00100309", "LearningAimsReference_Existing_Matching_Record");
        }

        [Fact]
        public async Task Then_Record_Is_Updated()
        {
            var learningAimsReferenceCount = _testFixture.MatchingDbContext.LearningAimsReference.Count();
            learningAimsReferenceCount.Should().Be(1);

            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.BulkImport(new LearningAimsReferenceStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(LearningAimsReferenceTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);

            learningAimsReferenceCount = _testFixture.MatchingDbContext.LearningAimsReference.Count();
            learningAimsReferenceCount.Should().Be(1);
        }

        public void Dispose()
        {
            _testFixture.ResetData();
            _testFixture?.Dispose();
        }
    }
}