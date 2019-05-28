using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService.DuplicateRows
{
    public class When_Import_Is_Called_To_Import_Valid_ProviderQualification_File_With_Duplicate_Rows : IClassFixture<FileImportServiceDuplicateRowsTestFixture<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>>
    {
        private readonly FileImportServiceDuplicateRowsTestFixture<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification> _fixture;
        public When_Import_Is_Called_To_Import_Valid_ProviderQualification_File_With_Duplicate_Rows(FileImportServiceDuplicateRowsTestFixture<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Twice()
        {
            _fixture.DataValidator.Received(2).ValidateAsync(Arg.Any<ProviderQualificationFileImportDto>());
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _fixture.FunctionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Twice()
        {
            _fixture.DataParser.Received(2).Parse(Arg.Any<ProviderQualificationFileImportDto>());
        }

        [Fact]
        public void Then_Repository_Create_Many_Is_called_With_Only_One_Item()
        {
            _fixture.Repository.Received(1).CreateMany(Arg.Is<IList<Domain.Models.ProviderQualification>>(arg => arg.Count == 1));
        }
    }
}