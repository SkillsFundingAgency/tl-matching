using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Constants;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Parsing
{
    public class When_Provider_Row_Is_Parsed : IClassFixture<ProviderParsingFixture>
    {
        private readonly IEnumerable<ProviderDto> _parseResult;
        private readonly ProviderDto _firstProviderDto;

        public When_Provider_Row_Is_Parsed(ProviderParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstProviderDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_UkPrn_Matches_Input() =>
            _firstProviderDto.UkPrn.Should().Be(long.Parse(ProviderConstants.UkPrn));

        [Fact]
        public void Then_First_ParseResult_Name_Matches_Input() =>
            _firstProviderDto.Name.Should().Be(ProviderConstants.Name);

        [Fact]
        public void Then_First_ParseResult_OfstedRating_Matches_Input() =>
            _firstProviderDto.OfstedRating.Should().Be(Enum.TryParse(ProviderConstants.OfstedRating, out OfstedRating _));

        [Fact]
        public void Then_First_ParseResult_Status_Matches_Input() =>
            _firstProviderDto.Status.Should().BeTrue();

        [Fact]
        public void Then_First_ParseResult_StatusReason_Matches_Input() =>
            _firstProviderDto.StatusReason.Should().Be(ProviderConstants.StatusReason);

        [Fact]
        public void Then_First_ParseResult_PrimaryContact_Matches_Input() =>
            _firstProviderDto.PrimaryContact.Should().Be(ProviderConstants.PrimaryContactName);

        [Fact]
        public void Then_First_ParseResult_PrimaryContactEmail_Matches_Input() =>
            _firstProviderDto.PrimaryContactEmail.Should().Be(ProviderConstants.PrimaryContactEmail);

        [Fact]
        public void Then_First_ParseResult_PrimaryContactPhone_Matches_Input() =>
            _firstProviderDto.PrimaryContactPhone.Should().Be(ProviderConstants.PrimaryContactTelephone);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactName_Matches_Input() =>
            _firstProviderDto.SecondaryContact.Should().Be(ProviderConstants.SecondaryContactName);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactEmail_Matches_Input() =>
            _firstProviderDto.SecondaryContactEmail.Should().Be(ProviderConstants.SecondaryContactEmail);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactPhone_Matches_Input() =>
            _firstProviderDto.SecondaryContactPhone.Should().Be(ProviderConstants.SecondaryContactTelephone);

        [Fact]
        public void Then_First_ParseResult_Source_Matches_Input() =>
            _firstProviderDto.Source.Should().Be(ProviderConstants.Source);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstProviderDto.CreatedBy.Should().Be(ProviderConstants.CreatedBy);
    }
}