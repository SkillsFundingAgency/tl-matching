using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathService_Imports_File : IClassFixture<QualificationRoutePathMappingTestFixture>
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-Simple.xlsx";
        private int _createdRecordCount;

        private readonly string _testExecutionDirectory;

        private readonly QualificationRoutePathMappingTestFixture _testFixture;
        public When_RoutePathService_Imports_File(QualificationRoutePathMappingTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.RouteMappingService.ImportQualificationPathMapping(new QualificationRoutePathMappingFileImportDto { FileDataStream = stream });
            }

            _createdRecordCount.Should().Be(3);
        }
    }
}
