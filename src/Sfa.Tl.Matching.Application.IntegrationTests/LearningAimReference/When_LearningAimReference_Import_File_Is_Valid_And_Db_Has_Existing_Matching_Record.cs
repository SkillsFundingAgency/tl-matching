using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimReference
{
    public class When_LearningAimReference_Import_File_Is_Valid_And_Db_Has_Existing_Matching_Record : IClassFixture<LearningAimReferenceTestFixture>, IDisposable
    {
        private readonly LearningAimReferenceTestFixture _testFixture;
        private const string DataFilePath = @"LearningAimReference\LearningAimReference_Existing_Matching_Record.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;

        public When_LearningAimReference_Import_File_Is_Valid_And_Db_Has_Existing_Matching_Record(LearningAimReferenceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
            _testFixture.AddExisting("00100309", "LearningAimReference_Existing_Matching_Record");
        }

        [Fact]
        public async Task Then_Record_Is_Updated()
        {
            var learningAimsReferenceCount = _testFixture.MatchingDbContext.LearningAimReference.Count();
            learningAimsReferenceCount.Should().Be(1);

            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.BulkImport(new LearningAimReferenceStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(LearningAimReferenceTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);

            learningAimsReferenceCount = _testFixture.MatchingDbContext.LearningAimReference.Count();
            learningAimsReferenceCount.Should().Be(1);
        }

        public void Dispose()
        {
            _testFixture.ResetData();
            _testFixture?.Dispose();
        }
    }
}