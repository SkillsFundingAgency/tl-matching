using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Parsing
{
    public class When_LearningAimReferenceStaging_Row_Is_Parsed : IClassFixture<LearningAimReferenceStagingParsingFixture>
    {
        private readonly IEnumerable<LearningAimReferenceStagingDto> _parseResult;
        private readonly LearningAimReferenceStagingDto _firstLearningAimReferenceStagingDto;

        public When_LearningAimReferenceStaging_Row_Is_Parsed(LearningAimReferenceStagingParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstLearningAimReferenceStagingDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_CrmId_Matches_Input() =>
            _firstLearningAimReferenceStagingDto.Title.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.Title);

        [Fact]
        public void Then_First_ParseResult_CompanyName_Matches_Input() =>
            _firstLearningAimReferenceStagingDto.LarId.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.LarId);

        [Fact]
        public void Then_First_ParseResult_AlsoKnownAs_Matches_Input() =>
            _firstLearningAimReferenceStagingDto.AwardOrgLarId.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.AwardOrgLarId);

        [Fact]
        public void Then_First_ParseResult_CompanyNameSearch_Should_Be_ComanyName_And_AlsoKnownAs_Combined_Containing_Letters_Or_Digits() =>
            _firstLearningAimReferenceStagingDto.SourceCreatedOn.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.SourceCreatedOn);

        [Fact]
        public void Then_First_ParseResult_Aupa_Matches_Input() =>
            _firstLearningAimReferenceStagingDto.SourceModifiedOn.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.SourceModifiedOn);

        [Fact]
        public void Then_First_ParseResult_CompanyType_Matches_Input() =>
            _firstLearningAimReferenceStagingDto.CreatedBy.Should().Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.CreatedBy);
    }
}