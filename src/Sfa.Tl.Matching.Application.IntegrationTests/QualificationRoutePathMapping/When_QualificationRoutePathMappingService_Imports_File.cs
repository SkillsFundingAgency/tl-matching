using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_QualificationRoutePathMappingService_Imports_File : IClassFixture<QualificationRoutePathMappingServiceTestFixture>, IDisposable
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\QualificationRoutePathMapping-Simple.xlsx";
        private int _createdRecordCount;
        private readonly string _testExecutionDirectory;
        private readonly QualificationRoutePathMappingServiceTestFixture _testFixture;
        private const string LarsId = "10000010";

        public When_QualificationRoutePathMappingService_Imports_File(QualificationRoutePathMappingServiceTestFixture testFixture)
        {
            _testFixture = testFixture;
            _testExecutionDirectory = TestConfiguration.GetTestExecutionDirectory();
            _testFixture.ResetData(LarsId);
        }

        [Fact]
        public async Task Then_Record_Is_Saved()
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
            //This is returning four because qualification object is also getting inserted
            _createdRecordCount.Should().Be(4);
        }

        public void Dispose()
        {
            _testFixture.ResetData(LarsId);
        }
    }
}