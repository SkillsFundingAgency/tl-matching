using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    [Trait("ValueParser", "Data Tests")]
    public class ValueParserDataTests
    {
        [Theory(DisplayName = "QualificationSearch Data Tests")]
        [InlineData("and", "")]
        [InlineData("BTEC Level", "")]
        [InlineData("Level 2 Diploma Health Social Care (Adults) for England", "DiplomaHealthSocialCareAdultsforEngland")]
        [InlineData("skills for professions in building and construction", "skillsforprofessionsbuildingconstruction")]
        [InlineData("BTEC skills for professions and BTEC skills for building and BTEC skills for construction", "skillsforprofessionsskillsforbuildingskillsforconstruction")]
        public void QualificationSearchDataTests(string searchTerm, string result)
        {
            searchTerm.ToQualificationSearch().Should().Be(result);
        }
    }
}