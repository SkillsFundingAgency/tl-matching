using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Parsing
{
    public class When_Employer_Row_Is_Parsed : IClassFixture<EmployerParsingFixture>
    {
        private readonly IEnumerable<EmployerStagingDto> _parseResult;
        private readonly EmployerStagingDto _firstEmployerStagingDto;

        public When_Employer_Row_Is_Parsed(EmployerParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstEmployerStagingDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_CrmId_Matches_Input() =>
            _firstEmployerStagingDto.CrmId.Should().Be(ValidEmployerFileImportDtoBuilder.CrmId);

        [Fact]
        public void Then_First_ParseResult_CompanyName_Matches_Input() =>
            _firstEmployerStagingDto.CompanyName.Should().Be(ValidEmployerFileImportDtoBuilder.Companyname);

        [Fact]
        public void Then_First_ParseResult_AlsoKnownAs_Matches_Input() =>
            _firstEmployerStagingDto.AlsoKnownAs.Should().Be(ValidEmployerFileImportDtoBuilder.Alsoknownas);

        [Fact]
        public void Then_First_ParseResult_CompanyNameSearch_Should_Be_ComanyName_And_AlsoKnownAs_Combined_Containing_Letters_Or_Digits() =>
            _firstEmployerStagingDto.CompanyNameSearch.Should().Be(ValidEmployerFileImportDtoBuilder.Companyname.ToLetterOrDigit() + ValidEmployerFileImportDtoBuilder.Alsoknownas.ToLetterOrDigit());

        [Fact]
        public void Then_First_ParseResult_Aupa_Matches_Input() =>
            _firstEmployerStagingDto.Aupa.Should().Be(ValidEmployerFileImportDtoBuilder.Aupa);

        [Fact]
        public void Then_First_ParseResult_CompanyType_Matches_Input() =>
            _firstEmployerStagingDto.CompanyType.Should().Be(ValidEmployerFileImportDtoBuilder.CompanyType);

        [Fact]
        public void Then_First_ParseResult_PrimaryContact_Matches_Input() =>
            _firstEmployerStagingDto.PrimaryContact.Should().Be(ValidEmployerFileImportDtoBuilder.PrimaryContact);

        [Fact]
        public void Then_First_ParseResult_Email_Matches_Input() =>
            _firstEmployerStagingDto.Email.Should().Be(ValidEmployerFileImportDtoBuilder.Email);

        [Fact]
        public void Then_First_ParseResult_Phone_Matches_Input() =>
            _firstEmployerStagingDto.Phone.Should().Be(ValidEmployerFileImportDtoBuilder.Phone);

        [Fact]
        public void Then_First_ParseResult_PostCode_Matches_Input() =>
            _firstEmployerStagingDto.Postcode.Should().Be(ValidEmployerFileImportDtoBuilder.Postcode);

        [Fact]
        public void Then_First_ParseResult_Owner_Matches_Input() =>
            _firstEmployerStagingDto.Owner.Should().Be(ValidEmployerFileImportDtoBuilder.Owner);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstEmployerStagingDto.CreatedBy.Should().Be(ValidEmployerFileImportDtoBuilder.CreatedBy);
    }
}