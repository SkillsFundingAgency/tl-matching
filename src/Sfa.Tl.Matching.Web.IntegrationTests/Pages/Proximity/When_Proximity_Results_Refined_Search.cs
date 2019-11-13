using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Proximity
{
    public class When_Proximity_Results_Refined_Search : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1010;
        private const int OpportunityItemId = 1011;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Proximity_Results_Refined_Search(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory(DisplayName = "Postcodes Updated Data Tests")]
        [InlineData("CV1 2WT", "CV1 2WT", true)]
        [InlineData("CV1 2WT", "CV1 1EE", false)]
        public async Task Then_Checkboxes_Are_Remained_Selected(string initialPostcode, string updatedPostcode, bool checkboxState)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-one-hour-of-{initialPostcode}-for-route-1");
            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);

            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var firstProviderCheckbox = pageContent.QuerySelector($"input[name='SelectedProvider[0].IsSelected']")
                as IHtmlInputElement;
            firstProviderCheckbox.IsChecked.Should().BeTrue();

            var secondProviderCheckbox = pageContent.QuerySelector($"input[name='SelectedProvider[1].IsSelected']")
                as IHtmlInputElement;
            secondProviderCheckbox.IsChecked.Should().BeFalse();

            var refinedSearchResponse = await client.SendAsync(
                (IHtmlFormElement)pageContent.GetElementById("tl-search-form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-search"),
                new Dictionary<string, string>
                {
                    ["Postcode"] = updatedPostcode
                });
            refinedSearchResponse.EnsureSuccessStatusCode();

            var refinedSearchContent = await HtmlHelpers.GetDocumentAsync(refinedSearchResponse);

            Assert.Equal(HttpStatusCode.OK, refinedSearchContent.StatusCode);

            firstProviderCheckbox = refinedSearchContent.QuerySelector($"input[name='SelectedProvider[0].IsSelected']")
                as IHtmlInputElement;
            firstProviderCheckbox.IsChecked.Should().Be(checkboxState);

            secondProviderCheckbox = refinedSearchContent.QuerySelector($"input[name='SelectedProvider[1].IsSelected']")
                as IHtmlInputElement;
            secondProviderCheckbox.IsChecked.Should().Be(false);
        }
    }
}