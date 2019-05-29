using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimsReference
{
    public class When_LearningAimsReference_Import_File_Is_Valid_And_Empty_Db_Add : IClassFixture<LearningAimsReferenceTestFixture>, IDisposable
    {
        private readonly LearningAimsReferenceTestFixture _testFixture;
        private const string DataFilePath = @"LearningAimsReference\LearningAimsReference-Empty-Db-Add.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private const string Title = "LearningAimsReference-Empty-Db-Add";

        public When_LearningAimsReference_Import_File_Is_Valid_And_Empty_Db_Add(LearningAimsReferenceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData();
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
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

            var learningAimsReference = _testFixture.MatchingDbContext.LearningAimsReference.FirstOrDefault(e => e.Title == Title);
            learningAimsReference.Should().NotBeNull();
        }

        public void Dispose()
        {
            _testFixture.ResetData(Title);
            _testFixture?.Dispose();
        }
    }
}