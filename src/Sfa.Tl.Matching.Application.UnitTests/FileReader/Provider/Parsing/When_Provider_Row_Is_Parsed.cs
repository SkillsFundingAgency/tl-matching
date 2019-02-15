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
        private readonly ProviderDto _firstEmployerDto;

        public When_Provider_Row_Is_Parsed(ProviderParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstEmployerDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_UkPrn_Matches_Input() =>
            _firstEmployerDto.UkPrn.Should().Be(long.Parse(ProviderConstants.UkPrn));

        [Fact]
        public void Then_First_ParseResult_Name_Matches_Input() =>
            _firstEmployerDto.Name.Should().Be(ProviderConstants.Name);

        [Fact]
        public void Then_First_ParseResult_OfstedRating_Matches_Input() =>
            _firstEmployerDto.OfstedRating.Should().Be(Enum.TryParse(ProviderConstants.OfstedRating, out OfstedRating _));

        [Fact]
        public void Then_First_ParseResult_Status_Matches_Input() =>
            _firstEmployerDto.Status.Should().BeTrue();

        [Fact]
        public void Then_First_ParseResult_StatusReason_Matches_Input() =>
            _firstEmployerDto.StatusReason.Should().Be(ProviderConstants.StatusReason);

        [Fact]
        public void Then_First_ParseResult_PrimaryContact_Matches_Input() =>
            _firstEmployerDto.PrimaryContact.Should().Be(ProviderConstants.PrimaryContactName);

        [Fact]
        public void Then_First_ParseResult_PrimaryContactEmail_Matches_Input() =>
            _firstEmployerDto.PrimaryContactEmail.Should().Be(ProviderConstants.PrimaryContactEmail);

        [Fact]
        public void Then_First_ParseResult_PrimaryContactPhone_Matches_Input() =>
            _firstEmployerDto.PrimaryContactPhone.Should().Be(ProviderConstants.PrimaryContactTelephone);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactName_Matches_Input() =>
            _firstEmployerDto.SecondaryContact.Should().Be(ProviderConstants.SecondaryContactName);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactEmail_Matches_Input() =>
            _firstEmployerDto.SecondaryContactEmail.Should().Be(ProviderConstants.SecondaryContactEmail);

        [Fact]
        public void Then_First_ParseResult_SecondaryContactPhone_Matches_Input() =>
            _firstEmployerDto.SecondaryContactPhone.Should().Be(ProviderConstants.SecondaryContactTelephone);

        [Fact]
        public void Then_First_ParseResult_Source_Matches_Input() =>
            _firstEmployerDto.Source.Should().Be(ProviderConstants.Source);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstEmployerDto.CreatedBy.Should().Be(ProviderConstants.CreatedBy);
    }
}