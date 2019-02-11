using System.IO;
using System.Threading.Tasks;

using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathService_Imports_File : IClassFixture<QualificationRoutePathMappingTestFixture>
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-Simple.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();

            await ResetData();

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await RouteMappingService.ImportQualificationPathMapping(new QualificationRoutePathMappingFileImportDto { FileDataStream = stream });
            }
        }

        [Test]
        public void Then_Record_Is_Saved()
        {
            Assert.AreEqual(3, _createdRecordCount);
        }
    }
}
