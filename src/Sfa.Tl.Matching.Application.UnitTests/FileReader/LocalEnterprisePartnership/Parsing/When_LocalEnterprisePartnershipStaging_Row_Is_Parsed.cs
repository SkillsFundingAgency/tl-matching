using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Parsing
{
    public class When_LocalEnterprisePartnershipStaging_Row_Is_Parsed : IClassFixture<LocalEnterprisePartnershipStagingParsingFixture>
    {
        private readonly IEnumerable<LocalEnterprisePartnershipStagingDto> _parseResult;
        private readonly LocalEnterprisePartnershipStagingDto _firstLocalEnterprisePartnershipNameMappingDto;

        public When_LocalEnterprisePartnershipStaging_Row_Is_Parsed(LocalEnterprisePartnershipStagingParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstLocalEnterprisePartnershipNameMappingDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_Fields_Match_Input()
        {
            _firstLocalEnterprisePartnershipNameMappingDto.Code
                .Should().Be("E37000007");
            _firstLocalEnterprisePartnershipNameMappingDto.Name
                .Should().Be("Cumbria");
            _firstLocalEnterprisePartnershipNameMappingDto.CreatedBy
                .Should().Be("CreatedBy");
        }
    }
}