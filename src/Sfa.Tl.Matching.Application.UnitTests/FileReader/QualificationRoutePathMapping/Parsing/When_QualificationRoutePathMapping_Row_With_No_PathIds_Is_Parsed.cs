﻿using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_QualificationRoutePathMapping_Row_With_No_PathIds_Is_Parsed : IClassFixture<QualificationRoutePathMappingParsingFixture>
    {
        private readonly IEnumerable<QualificationRoutePathMappingDto> _parseResult;

        public When_QualificationRoutePathMapping_Row_With_No_PathIds_Is_Parsed(QualificationRoutePathMappingParsingFixture fixture)
        {
            fixture.Dto.Accountancy = null;
            _parseResult = fixture.Parser.Parse(fixture.Dto);
        }

        [Fact]
        public void Then_ParseResult_Count_Is_Zero() =>
            _parseResult.Should().BeEmpty();
    }
}