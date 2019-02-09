using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_File_With_Wrong_Data_Type : ProviderTestBase
    {
        private const string DataFilePath = @"Provider\Provider-WrongDataType.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public async Task Setup()
        {
            await ResetData();

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = _providerService.ImportProvider(stream).Result;
            }
        }

        [Test]
        public void Then_No_Record_Is_Saved()
        {
            Assert.AreEqual(0, _createdRecordCount);
        }
    }
}