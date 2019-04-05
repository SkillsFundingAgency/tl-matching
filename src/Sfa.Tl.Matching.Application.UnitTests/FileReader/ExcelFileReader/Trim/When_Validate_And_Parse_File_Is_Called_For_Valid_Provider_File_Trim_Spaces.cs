using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_Provider_File_Trim_Spaces : IClassFixture<ExcelFileReaderTrimDataTestFixture<ProviderFileImportDto, ProviderDto>>
    {
        private readonly ExcelFileReaderTrimDataTestFixture<ProviderFileImportDto, ProviderDto> _fixture;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_Provider_File_Trim_Spaces(ExcelFileReaderTrimDataTestFixture<ProviderFileImportDto, ProviderDto> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once_And_Leading_And_Trailing_Speaces_are_Trimmed()
        {
            _fixture.DataValidator.Received(1).ValidateAsync(Arg.Is<ProviderFileImportDto>(arg =>
                arg.UkPrn == "10000001" && 
                arg.ProviderName == "Provider-Trim" && 
                arg.OfstedRating == "Good" &&
                arg.Status == "Yes" &&
                arg.StatusReason == "Active Reason" &&
                arg.PrimaryContactName == "PrimaryContact" &&
                arg.PrimaryContactEmail == "primary@contact.co.uk" &&
                arg.PrimaryContactTelephone == "01777757777" &&
                arg.SecondaryContactName == "SecondaryContact" &&
                arg.SecondaryContactEmail == "secondary@contact.co.uk" &&
                arg.SecondaryContactTelephone == "01777757777" &&
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
            _fixture.DataParser.Received(1).Parse(Arg.Any<ProviderFileImportDto>());
        }

        [Fact]
        public void Then_Returned_List_Has_One_Item()
        {
            _fixture.Results.Should().NotBeNullOrEmpty();

            _fixture.Results.Count.Should().Be(1);

            var dto = _fixture.Results.ElementAt(0);

            dto.UkPrn.Should().Be(10000001); 
            dto.Name.Should().Be("Provider-Trim"); 
            dto.OfstedRating.Should().Be(OfstedRating.Good);
            dto.Status.Should().BeTrue();
            dto.StatusReason.Should().Be("Active Reason");
            dto.PrimaryContact.Should().Be("PrimaryContact");
            dto.PrimaryContactEmail.Should().Be("primary@contact.co.uk");
            dto.PrimaryContactPhone.Should().Be("01777757777");
            dto.SecondaryContact.Should().Be("SecondaryContact");
            dto.SecondaryContactEmail.Should().Be("secondary@contact.co.uk");
            dto.SecondaryContactPhone.Should().Be("01777757777");
            dto.Source.Should().Be("PMF_1018");
        }
    }
}