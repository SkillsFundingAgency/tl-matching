using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathService_Imports_File_And_Records_Already_Exist : IClassFixture<RoutePathMappingServiceTestFixture>
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-RecordsAlreadyExist.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly RoutePathMappingServiceTestFixture _testFixture;
        private const string LarsId = "10000002";

        public When_RoutePathService_Imports_File_And_Records_Already_Exist(RoutePathMappingServiceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData(LarsId);
            _testFixture.CreateRoutePathMapping(LarsId);
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.RouteMappingService.ImportQualificationPathMapping(new QualificationRoutePathMappingFileImportDto { FileDataStream = stream });
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}