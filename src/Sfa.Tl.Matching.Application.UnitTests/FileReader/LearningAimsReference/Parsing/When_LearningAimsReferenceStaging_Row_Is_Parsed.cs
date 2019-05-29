using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Parsing
{
    public class When_LearningAimsReferenceStaging_Row_Is_Parsed : IClassFixture<LearningAimsReferenceStagingParsingFixture>
    {
        private readonly IEnumerable<LearningAimsReferenceStagingDto> _parseResult;
        private readonly LearningAimsReferenceStagingDto _firstLearningAimsReferenceStagingDto;

        public When_LearningAimsReferenceStaging_Row_Is_Parsed(LearningAimsReferenceStagingParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstLearningAimsReferenceStagingDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_CrmId_Matches_Input() =>
            _firstLearningAimsReferenceStagingDto.Title.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.Title);

        [Fact]
        public void Then_First_ParseResult_CompanyName_Matches_Input() =>
            _firstLearningAimsReferenceStagingDto.LarId.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.LarId);

        [Fact]
        public void Then_First_ParseResult_AlsoKnownAs_Matches_Input() =>
            _firstLearningAimsReferenceStagingDto.AwardOrgLarId.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.AwardOrgLarId);

        [Fact]
        public void Then_First_ParseResult_CompanyNameSearch_Should_Be_ComanyName_And_AlsoKnownAs_Combined_Containing_Letters_Or_Digits() =>
            _firstLearningAimsReferenceStagingDto.SourceCreatedOn.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.SourceCreatedOn);

        [Fact]
        public void Then_First_ParseResult_Aupa_Matches_Input() =>
            _firstLearningAimsReferenceStagingDto.SourceModifiedOn.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.SourceModifiedOn);

        [Fact]
        public void Then_First_ParseResult_CompanyType_Matches_Input() =>
            _firstLearningAimsReferenceStagingDto.CreatedBy.Should().Be(ValidLearningAimsReferenceStagingFileImportDtoBuilder.CreatedBy);
    }
}