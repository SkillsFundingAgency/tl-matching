using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Import_File_Is_Valid : EmployerTestBase
    {
        private const string DataFilePath = @"Employer\Employer-Simple.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public async Task Setup()
        {
            await ResetData();

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = EmployerService.ImportEmployer(new EmployerFileImportDto { FileDataStream = stream }).Result;
            }
        }

        [Test]
        public void Then_Record_Is_Saved()
        {
            Assert.AreEqual(1, _createdRecordCount);
        }
    }
}