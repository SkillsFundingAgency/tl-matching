using System.IO;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_File_With_Missing_Mandatory : IClassFixture<ProviderTestFixture>
    {
        private const string DataFilePath = @"Provider\Provider-MissingMandatory.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly ProviderTestFixture _testFixture;
        private const string ProviderName = "Provider-MissingMandatory";

        public When_Provider_Imports_File_With_Missing_Mandatory(ProviderTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();

            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = _testFixture.ProviderService.ImportProvider(new ProviderFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(ProviderTestFixture)
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Record_Is_Saved() =>
           _createdRecordCount.Should().Be(0);

        [Fact]
        public void Then_Record_Does_Not_Exist() =>
            _testFixture.GetCountBy(ProviderName).Should().Be(0);
    }
}