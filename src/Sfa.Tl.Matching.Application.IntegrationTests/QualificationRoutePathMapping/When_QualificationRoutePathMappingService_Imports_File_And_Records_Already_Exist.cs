using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingService_Imports_File_And_Records_Already_Exist : IClassFixture<QualificationRoutePathMappingServiceTestFixture>, IDisposable
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\QualificationRoutePathMapping-RecordsAlreadyExist.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly QualificationRoutePathMappingServiceTestFixture _testFixture;
        private const string LarsId = "10000002";

        public When_QualificationRoutePathMappingService_Imports_File_And_Records_Already_Exist(QualificationRoutePathMappingServiceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestHelper.GetTestExecutionDirectory();
            _testFixture.ResetData(LarsId);
            _testFixture.CreateQualificationRoutePathMapping(LarsId);
        }

        [Fact]
        public async Task Then_No_Record_Is_Saved()
        {
            var filePath = Path.Combine(_testExecutionDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await _testFixture.FileImportService.Import(new QualificationRoutePathMappingFileImportDto
                {
                    FileDataStream = stream,
                    CreatedBy = nameof(QualificationRoutePathMappingServiceTestFixture)
                });
            }

            _createdRecordCount.Should().Be(0);
        }

        public void Dispose()
        {
            _testFixture.ResetData(LarsId);
        }
    }
}