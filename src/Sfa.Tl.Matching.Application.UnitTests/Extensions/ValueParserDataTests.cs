using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Configuration;
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
        [InlineData("and BTEC LEVEL", "")]
        [InlineData("and BTEC Level", "")]
        [InlineData("city & guilds", "")]
        [InlineData("Level 2 Diploma Health Social Care (Adults) for England", "HealthSocialCareAdultsforEngland")]
        [InlineData("skills for professions in building and construction", "skillsforprofessionsbuildingconstruction")]
        [InlineData("BTEC skills for professions and BTEC skills for building and BTEC skills for construction", "skillsforprofessionsskillsforbuildingskillsforconstruction")]
        [InlineData("BTEC skills city & guilds professions", "skillsprofessions")]
        public void QualificationSearchDataTests(string searchTerm, string result)
        {
            searchTerm.ToQualificationSearch().Should().Be(result);
        }


        [Theory(DisplayName = "AllSpecialCharactersOrNumbers Data Tests")]
        [InlineData("Test", false)]
        [InlineData("Test2342423", false)]
        [InlineData("Test Test2", false)]
        [InlineData("Test Test2 Test3 Test4", false)]
        [InlineData("Test 4234 Test2 33", false)]
        [InlineData("$£%$£$ $£$", true)]
        [InlineData("123213", true)]
        [InlineData("$", true)]
        [InlineData("$£%$£$", true)]
        public void AllSpecialCharactersOrNumbersDataTests(string searchTerm, bool result)
        {
            searchTerm.IsAllSpecialCharactersOrNumbers().Should().Be(result);
        }
    }
}