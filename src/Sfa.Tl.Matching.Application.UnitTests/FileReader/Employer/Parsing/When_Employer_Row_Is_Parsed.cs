using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Parsing
{
    public class When_Employer_Row_Is_Parsed : IClassFixture<EmployerParsingFixture>
    {
        private readonly IEnumerable<EmployerDto> _parseResult;
        private readonly EmployerDto _firstEmployerDto;

        public When_Employer_Row_Is_Parsed(EmployerParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstEmployerDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_CrmId_Matches_Input() =>
            _firstEmployerDto.CrmId.Should().Be(ValidEmployerFileImportDtoBuilder.CrmId);

        [Fact]
        public void Then_First_ParseResult_CompanyName_Matches_Input() =>
            _firstEmployerDto.CompanyName.Should().Be(ValidEmployerFileImportDtoBuilder.Companyname);

        [Fact]
        public void Then_First_ParseResult_AlsoKnownAs_Matches_Input() =>
            _firstEmployerDto.AlsoKnownAs.Should().Be(ValidEmployerFileImportDtoBuilder.Alsoknownas);

        [Fact]
        public void Then_First_ParseResult_Aupa_Matches_Input() =>
            _firstEmployerDto.Aupa.Should().Be(ValidEmployerFileImportDtoBuilder.Aupa);

        [Fact]
        public void Then_First_ParseResult_CompanyType_Matches_Input() =>
            _firstEmployerDto.CompanyType.Should().Be(ValidEmployerFileImportDtoBuilder.CompanyType);

        [Fact]
        public void Then_First_ParseResult_PrimaryContact_Matches_Input() =>
            _firstEmployerDto.PrimaryContact.Should().Be(ValidEmployerFileImportDtoBuilder.PrimaryContact);

        [Fact]
        public void Then_First_ParseResult_Email_Matches_Input() =>
            _firstEmployerDto.Email.Should().Be(ValidEmployerFileImportDtoBuilder.Email);

        [Fact]
        public void Then_First_ParseResult_Phone_Matches_Input() =>
            _firstEmployerDto.Phone.Should().Be(ValidEmployerFileImportDtoBuilder.Phone);

        [Fact]
        public void Then_First_ParseResult_PostCode_Matches_Input() =>
            _firstEmployerDto.Postcode.Should().Be(ValidEmployerFileImportDtoBuilder.Postcode);

        [Fact]
        public void Then_First_ParseResult_Owner_Matches_Input() =>
            _firstEmployerDto.Owner.Should().Be(ValidEmployerFileImportDtoBuilder.Owner);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstEmployerDto.CreatedBy.Should().Be(ValidEmployerFileImportDtoBuilder.CreatedBy);
    }
}