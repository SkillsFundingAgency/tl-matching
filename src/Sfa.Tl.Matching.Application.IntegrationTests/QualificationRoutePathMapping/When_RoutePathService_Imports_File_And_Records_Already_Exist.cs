using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathService_Imports_File_And_Records_Already_Exist
        : RoutePathMappingServiceTestBase
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-Simple.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public override async Task Setup()
        {
            await base.Setup();

            await ResetData();

            MatchingDbContext.Add(new Domain.Models.RoutePathMapping
            {
                LarsId = "60144567", //Must match id in RoutePathMapping-Simple.xlsx
                Title = "Test",
                PathId = 1
            });
            await MatchingDbContext.SaveChangesAsync();
            
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await RouteMappingService.ImportQualificationPathMapping(stream);
            }
        }

        [Test]
        public void Then_Record_Is_Not_Saved()
        {
            Assert.AreEqual(0, _createdRecordCount);
        }
    }
}
