using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService.DuplicateRows
{
    public class When_Import_Is_Called_To_Import_Valid_EmployerStaging_File_With_Duplicate_Rows : IClassFixture<ExcelFileImportServiceDuplicateRowsTestFixture<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging>>
    {
        private readonly ExcelFileImportServiceDuplicateRowsTestFixture<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging> _fixture;

        public When_Import_Is_Called_To_Import_Valid_EmployerStaging_File_With_Duplicate_Rows(ExcelFileImportServiceDuplicateRowsTestFixture<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_Called_Exactly_Twice()
        {
            _fixture.DataValidator.Received(2).ValidateAsync(Arg.Any<EmployerStagingFileImportDto>());
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _fixture.FunctionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Twice()
        {
            _fixture.DataParser.Received(2).Parse(Arg.Any<EmployerStagingFileImportDto>());
        }

        [Fact]
        public void Then_Repository_Create_Many_Is_Called_With_Only_One_Item()
        {
            _fixture.Repository.Received(1).BulkInsert(Arg.Is<IList<EmployerStaging>>(arg => arg.Count == 1));
        }
    }
}
