using FluentAssertions;
using Sfa.Tl.Matching.Web.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Extensions
{
    [Trait("PageExtensions", "Generate Title")]
    public class PageExtensionsDataTests
    {
        [Theory(DisplayName = "Data Tests")]
        [InlineData(true, null, "Match employers with providers for industry placements - GOV.UK")]
        [InlineData(true, "", "Match employers with providers for industry placements - GOV.UK")]
        [InlineData(true, "Match employers with providers for industry placements", "Match employers with providers for industry placements - GOV.UK")]
        [InlineData(true, "Page Title", "Page Title - Match employers with providers for industry placements - GOV.UK")]
        [InlineData(false, null, "Error: Match employers with providers for industry placements - GOV.UK")]
        [InlineData(false, "", "Error: Match employers with providers for industry placements - GOV.UK")]
        [InlineData(false, "Match employers with providers for industry placements", "Error: Match employers with providers for industry placements - GOV.UK")]
        [InlineData(false, "Page Title", "Error: Page Title - Match employers with providers for industry placements - GOV.UK")]
        public void DataTests(bool isValid, string title, string result)
        {
            var generatedTitle = PageExtensions.GenerateTitle(isValid, title);

            generatedTitle.Should().Be(result);
        }
    }
}