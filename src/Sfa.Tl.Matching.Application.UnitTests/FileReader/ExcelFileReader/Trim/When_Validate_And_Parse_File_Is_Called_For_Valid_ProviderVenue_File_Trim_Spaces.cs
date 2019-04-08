using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_ProviderVenue_File_Trim_Spaces : IClassFixture<ExcelFileReaderTrimDataTestFixture<ProviderVenueFileImportDto, ProviderVenueDto>>
    {
        private readonly ExcelFileReaderTrimDataTestFixture<ProviderVenueFileImportDto, ProviderVenueDto> _fixture;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_ProviderVenue_File_Trim_Spaces(ExcelFileReaderTrimDataTestFixture<ProviderVenueFileImportDto, ProviderVenueDto> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once_And_Leading_And_Trailing_Speaces_are_Trimmed()
        {
            _fixture.DataValidator.Received(1).ValidateAsync(Arg.Is<ProviderVenueFileImportDto>(arg =>
                arg.UkPrn == "10000546" && 
                arg.Postcode == "CV1 2WT" && 
                arg.Source == "PMF_1018"
                ));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _fixture.FunctionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once()
        {
            _fixture.DataParser.Received(1).Parse(Arg.Any<ProviderVenueFileImportDto>());
        }

        [Fact]
        public void Then_Returned_List_Has_One_Item()
        {
            _fixture.Results.Should().NotBeNullOrEmpty();

            _fixture.Results.Count.Should().Be(1);

            var dto = _fixture.Results.ElementAt(0);

            dto.ProviderId.Should().Be(0); 
            dto.Postcode.Should().Be("CV1 2WT"); 
            dto.Source.Should().Be("PMF_1018"); 
        }
    }
}