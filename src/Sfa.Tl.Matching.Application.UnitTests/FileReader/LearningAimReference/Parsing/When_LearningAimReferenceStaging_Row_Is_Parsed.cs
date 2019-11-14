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
        public void Then_First_ParseResult_Fields_Match_Input()
        {
            _firstLearningAimReferenceStagingDto.Title.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.Title);
            _firstLearningAimReferenceStagingDto.LarId.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.LarId);
            _firstLearningAimReferenceStagingDto.AwardOrgLarId.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.AwardOrgLarId);
            _firstLearningAimReferenceStagingDto.SourceCreatedOn.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.SourceCreatedOn);
            _firstLearningAimReferenceStagingDto.SourceModifiedOn.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.SourceModifiedOn);
            _firstLearningAimReferenceStagingDto.CreatedBy.Should()
                .Be(ValidLearningAimReferenceStagingFileImportDtoBuilder.CreatedBy);
        }
    }
}