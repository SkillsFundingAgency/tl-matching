using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_Multiple_PathIds_Is_Parsed : IClassFixture<QualificationRoutePathMappingParsingFixture>
    {
        private readonly IEnumerable<RoutePathMappingDto> _parseResult;

        public When_RoutePathMapping_Row_With_Multiple_PathIds_Is_Parsed(QualificationRoutePathMappingParsingFixture fixture)
        {
            fixture.Dto.Accountancy = null;
            fixture.Dto.AnimalCareandManagement = ValidQualificationRoutePathMappingFileImportDtoBuilder.AnimalCareandManagement;
            fixture.Dto.Hospitality = ValidQualificationRoutePathMappingFileImportDtoBuilder.Hospitality;

            _parseResult = fixture.Parser.Parse(fixture.Dto);
        }

        [Fact]
        public void Then_ParseResult_Count_Is_Two() =>
            _parseResult.Count().Should().Be(2);

        [Fact]
        public void Then_First_ParseResult_LarsId_Matches_Input() =>
            _parseResult.First().LarsId.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.LarsId);

        [Fact]
        public void Then_First_ParseResult_Title_Matches_Input() =>
            _parseResult.First().Title.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.Title);

        [Fact]
        public void Then_First_ParseResult_ShortTitle_Matches_Input() =>
            _parseResult.First().ShortTitle.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.ShortTitle);

        [Fact]
        public void Then_First_ParseResult_PathId_Matches_Input() =>
            _parseResult.First().PathId.Should().Be(int.Parse(ValidQualificationRoutePathMappingFileImportDtoBuilder.AnimalCareandManagement));

        [Fact]
        public void Then_Second_ParseResult_LarsId_Matches_Input() =>
            Assert.Equal(ValidQualificationRoutePathMappingFileImportDtoBuilder.LarsId, _parseResult.Skip(1).First().LarsId);

        [Fact]
        public void Then_Second_ParseResult_Title_Matches_Input() =>
            _parseResult.Skip(1).First().Title.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.Title);

        [Fact]
        public void Then_Second_ParseResult_ShortTitle_Matches_Input() =>
            _parseResult.Skip(1).First().ShortTitle.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.ShortTitle);

        [Fact]
        public void Then_Second_First_ParseResult_PathId_Matches_Input() =>
            _parseResult.Skip(1).First().PathId.Should().Be(int.Parse(ValidQualificationRoutePathMappingFileImportDtoBuilder.Hospitality));
    }
}