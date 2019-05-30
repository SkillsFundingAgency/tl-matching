using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_Employer_File_Trim_Spaces : IClassFixture<ExcelFileReaderTrimDataTestFixture<EmployerStagingFileImportDto, EmployerStagingDto>>
    {
        private readonly ExcelFileReaderTrimDataTestFixture<EmployerStagingFileImportDto, EmployerStagingDto> _fixture;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_Employer_File_Trim_Spaces(ExcelFileReaderTrimDataTestFixture<EmployerStagingFileImportDto, EmployerStagingDto> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once_And_Leading_And_Trailing_Speaces_are_Trimmed()
        {
            _fixture.DataValidator.Received(1).ValidateAsync(Arg.Is<EmployerStagingFileImportDto>(arg =>
                arg.CrmId == "9082609f-9cf8-e811-80e0-000d3a214f60" && 
                arg.CompanyName == "HARDIK DESAI LTD" && 
                arg.AlsoKnownAs == "also known as" &&
                arg.Aupa == "Aware" &&
                arg.CompanyType == "Employer" &&
                arg.PrimaryContact == "Primary Contact" &&
                arg.Phone == "7777744465" &&
                arg.Email == "email@address.com" &&
                arg.Postcode == "S1 1AA" &&
                arg.Owner == "Owner"
                ));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _fixture.FunctionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once_And_Company_Name_And_Also_KnownAs_Fields_are_Converted_To_TitleCase()
        {
            _fixture.DataParser.Received(1).Parse(Arg.Any<EmployerStagingFileImportDto>());
        }

        [Fact]
        public void Then_Returned_List_Has_One_Item_And_Company_Name_And_Also_KnownAs_Fields_are_Converted_To_TitleCase()
        {
            _fixture.Results.Should().NotBeNullOrEmpty();

            _fixture.Results.Count.Should().Be(1);

            var dto = _fixture.Results.ElementAt(0);

            dto.CrmId.Should().Be("9082609f-9cf8-e811-80e0-000d3a214f60");
            dto.CompanyName.Should().Be("Hardik Desai Ltd");
            dto.AlsoKnownAs.Should().Be("Also Known As");
            dto.Aupa.Should().Be("Aware");
            dto.CompanyType.Should().Be("Employer");
            dto.PrimaryContact.Should().Be("Primary Contact");
            dto.Phone.Should().Be("7777744465");
            dto.Email.Should().Be("email@address.com");
            dto.Postcode.Should().Be("S1 1AA");
            dto.Owner.Should().Be("Owner");
        }
    }
}