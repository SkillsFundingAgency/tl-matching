using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimReference
{
    public class When_LearningAimReference_Import_File_Is_Valid_And_Empty_Db_Add : IClassFixture<LearningAimReferenceTestFixture>, IDisposable
    {
        private readonly LearningAimReferenceTestFixture _testFixture;
        private const string DataFilePath = @"LearningAimReference\LearningAimReference-Empty-Db-Add.csv";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private const string Title = "LearningAimReference-Empty-Db-Add";

        public When_LearningAimReference_Import_File_Is_Valid_And_Empty_Db_Add(LearningAimReferenceTestFixture testFixture)
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
                _createdRecordCount = await _testFixture.FileImportService.BulkImport(new LearningAimReferenceStagingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(LearningAimReferenceTestFixture)
                });
            }

            _createdRecordCount.Should().Be(1);

            var learningAimsReference = _testFixture.MatchingDbContext.LearningAimReference.FirstOrDefault(e => e.Title == Title);
            learningAimsReference.Should().NotBeNull();
        }

        public void Dispose()
        {
            _testFixture.ResetData(Title);
            _testFixture?.Dispose();
        }
    }
}