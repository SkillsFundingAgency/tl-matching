using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Constants;
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
            _firstEmployerDto.CrmId.Should().Be(EmployerConstants.CrmId);

        [Fact]
        public void Then_First_ParseResult_CompanyName_Matches_Input() =>
            _firstEmployerDto.CompanyName.Should().Be(EmployerConstants.CompanyName);

        [Fact]
        public void Then_First_ParseResult_AlsoKnownAs_Matches_Input() =>
            _firstEmployerDto.AlsoKnownAs.Should().Be(EmployerConstants.AlsoKnownAs);

        [Fact]
        public void Then_First_ParseResult_Aupa_Matches_Input() =>
            _firstEmployerDto.Aupa.Should().Be(EmployerConstants.Aupa);

        [Fact]
        public void Then_First_ParseResult_CompanyType_Matches_Input() =>
            _firstEmployerDto.CompanyType.Should().Be(EmployerConstants.CompanyType);

        [Fact]
        public void Then_First_ParseResult_PrimaryContact_Matches_Input() =>
            _firstEmployerDto.PrimaryContact.Should().Be(EmployerConstants.PrimaryContact);

        [Fact]
        public void Then_First_ParseResult_Email_Matches_Input() =>
            _firstEmployerDto.Email.Should().Be(EmployerConstants.Email);

        [Fact]
        public void Then_First_ParseResult_Phone_Matches_Input() =>
            _firstEmployerDto.Phone.Should().Be(EmployerConstants.Phone);

        [Fact]
        public void Then_First_ParseResult_PostCode_Matches_Input() =>
            _firstEmployerDto.PostCode.Should().Be(EmployerConstants.PostCode);

        [Fact]
        public void Then_First_ParseResult_Owner_Matches_Input() =>
            _firstEmployerDto.Owner.Should().Be(EmployerConstants.Owner);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstEmployerDto.CreatedBy.Should().Be(EmployerConstants.CreatedBy);
    }
}