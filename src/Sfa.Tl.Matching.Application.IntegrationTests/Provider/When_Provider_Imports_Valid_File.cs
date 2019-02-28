using System.IO;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_Valid_File : IClassFixture<ProviderTestFixture>
    {
        private const string DataFilePath = @"Provider\Provider-Simple.xlsx";
        private readonly int _createdRecordCount;
        private readonly ProviderTestFixture _testFixture;
        private const string ProviderName = "Provider-Simple";

        public When_Provider_Imports_Valid_File(ProviderTestFixture testFixture)
        {
            _testFixture = testFixture;
            var testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData();

            var filePath = Path.Combine(testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = _testFixture.FileImportService.Import(new ProviderFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(ProviderTestFixture)
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Record_Is_Saved() =>
            _createdRecordCount.Should().Be(1);

        [Fact]
        public void Then_Record_Does_Exist() =>
            _testFixture.GetCountBy(ProviderName).Should().Be(1);
    }
}