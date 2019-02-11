using System.IO;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class When_Employer_Import_File_Has_Missing_Mandatory : IClassFixture<EmployerTestFixture>
    {
        private const string DataFilePath = @"Employer\Employer-MissingMandatory.xlsx";
        private int _createdRecordCount;

        [Fact]
        public async Task Then_No_Record_Is_Saved(IEmployerService EmployerService,  MatchingDbContext MatchingDbContext, string TestExecutionDirectory)
        {
            //await ResetData();

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = EmployerService.ImportEmployer(new EmployerFileImportDto { FileDataStream = stream }).Result;
            }

            _createdRecordCount.Should().Be(0);
        }
    }
}