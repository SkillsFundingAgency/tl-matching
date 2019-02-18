using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathMappingService_Imports_File : IClassFixture<RoutePathMappingServiceTestFixture>
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-Simple.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly RoutePathMappingServiceTestFixture _testFixture;
        private const string LarsId = "10000001";

        public When_RoutePathMappingService_Imports_File(RoutePathMappingServiceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData(LarsId);
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.RouteMappingService.ImportQualificationPathMapping(new QualificationRoutePathMappingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(RoutePathMappingServiceTestFixture)
                });
            }

            _createdRecordCount.Should().Be(3);
        }
    }
}