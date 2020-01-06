using FluentAssertions;
using Sfa.Tl.Matching.Models.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    [Trait("ProviderDisplayExtensions", "Data Tests")]
    public class ProviderDisplayExtensionsTests
    {
        [Theory(DisplayName = "GetDisplayText Data Tests")]
        [InlineData("venue", "CV1 2WT", "display name", true, "venue part of display name (CV1 2WT)")]
        [InlineData("CV1 2WT", "CV1 2WT", "display name", true, "display name (CV1 2WT)")]
        [InlineData("cv1 2wt", "CV1 2WT", "display name", true, "display name (CV1 2WT)")]
        public void GetDisplayTextDataTests(string venue, string postcode, string displayName, bool includePartOf, string result)
        {
            var displayText = ProviderDisplayExtensions.GetDisplayText(venue, postcode, displayName, includePartOf);
            displayText.Should().Be(result);
        }
        
        [Theory(DisplayName = "GetProviderEmailDisplayText Data Tests")]
        [InlineData("venue name", "CV1 2WT", "display name", "venue name")]
        [InlineData("CV1 2WT", "CV1 2WT", "display name", "display name")]
        [InlineData("cv1 2wt", "CV1 2WT", "display name", "display name")]
        public void GetProviderEmailDisplayTextDataTests(string venue, string postcode, string displayName, string result)
        {
            var displayText = ProviderDisplayExtensions.GetProviderEmailDisplayText(venue, postcode, displayName);
            displayText.Should().Be(result);
        }
    }
}