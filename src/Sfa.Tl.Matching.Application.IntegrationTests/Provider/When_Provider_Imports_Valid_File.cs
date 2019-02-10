using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_Valid_File : ProviderTestBase
    {
        private const string DataFilePath = @"Provider\Provider-Simple.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public async Task Setup()
        {
            await ResetData();

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = ProviderService.ImportProvider(new ProviderFileImportDto { FileDataStream = stream }).Result;
            }
        }

        [Test]
        public void Then_Record_Is_Saved()
        {
            Assert.AreEqual(1, _createdRecordCount);
        }
    }
}