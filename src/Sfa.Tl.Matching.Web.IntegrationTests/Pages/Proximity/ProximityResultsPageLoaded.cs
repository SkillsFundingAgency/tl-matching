﻿using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Proximity
{
    public class ProximityResultsPageLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Select providers for this opportunity";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public ProximityResultsPageLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-10-miles-of-CV1%202WT-for-route-1");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var cancelLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            cancelLink.PathName.Should().Be($"/remove-opportunityItem/{OpportunityId}-{OpportunityItemId}");

            var routeList = documentHtml.GetElementById("SelectedRouteId");
            routeList.Children.Length.Should().Be(2);
            routeList.TextContent.Should().Be("Agriculture, environmental and animal care\nBusiness and administration\n");

            var postcode = documentHtml.GetElementById("Postcode") as IHtmlInputElement;
            postcode.Value.Should().Be("CV1 2WT");

            var searchRadiusList = documentHtml.GetElementById("SearchRadius") as IHtmlSelectElement;
            searchRadiusList.Children.Length.Should().Be(5);
            searchRadiusList.Value.Should().Be("10");

            var searchButton = documentHtml.GetElementById("tl-search") as IHtmlButtonElement;
            searchButton.TextContent.Should().Be("Search again");
            searchButton.Value.Should().Be("searchAgain");
            searchButton.Type.Should().Be("submit");

            var searchCount = documentHtml.GetElementById("tl-search-count");
            searchCount.TextContent.Should().Be("2");

            var searchSkillArea = documentHtml.GetElementById("tl-search-skillarea");
            searchSkillArea.TextContent.Should().Be("Agriculture, environmental and animal care");

            var searchDistance = documentHtml.GetElementById("tl-search-distance");
            searchDistance.TextContent.Should().Be("10 miles");

            var searchPostcode = documentHtml.GetElementById("tl-search-postcode");
            searchPostcode.TextContent.Should().Be("CV1 2WT");

            var noProvidersLink = documentHtml.GetElementById("tl-search-nosuitable") as IHtmlAnchorElement;
            noProvidersLink.Text.Should().Be("No suitable providers? Let us know");
            noProvidersLink.PathName.Should().Be($"/2-provisiongap-opportunities-within-10-miles-of-CV1%202WT-for-route-1");

            var searchResults = documentHtml.QuerySelector(".tl-search-results") as IHtmlOrderedListElement;
            AssertSearchResult(searchResults, 0);

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue with selected providers");
            continueButton.Type.Should().Be("submit");
        }

        private void AssertSearchResult(IHtmlOrderedListElement searchResults, int itemNumber)
        {
            var searchDistance = searchResults.Children[itemNumber].QuerySelector("#searchResult_Distance") as IHtmlInputElement;
            searchDistance.Type.Should().Be("hidden");
            searchDistance.Value.Should().Be("0");

            var providerVenueId = searchResults.Children[itemNumber].QuerySelector("#searchResult_ProviderVenueId") as IHtmlInputElement;
            providerVenueId.Type.Should().Be("hidden");
            providerVenueId.Value.Should().Be("1");

            var providerIsSelected = searchResults.Children[itemNumber].QuerySelector($"#SearchResults_Results_{itemNumber}__IsSelected") as IHtmlInputElement;
            providerIsSelected.Type.Should().Be("checkbox");
            providerIsSelected.Value.Should().Be("true");

            var tLevelProvider = searchResults.Children[itemNumber].QuerySelector("#tl-provider");
            tLevelProvider.TextContent.Should().Be("T level provider");
            tLevelProvider.ClassName.Should().Be("tl-search-results-flag");

            var venueDetailList = searchResults.Children[itemNumber].QuerySelector("#tl-venue-detail-list");
            var providerNameListItem = venueDetailList.Children[0] as IHtmlListItemElement;
            providerNameListItem.TextContent.Should().Be("Part of SQL Search Provider Display Name");

            var townAndPostcodeListItem = venueDetailList.Children[1] as IHtmlListItemElement;
            townAndPostcodeListItem.TextContent.Should().Be("Coventry CV1 2WT");

            var distanceListItem = venueDetailList.Children[2] as IHtmlListItemElement;
            distanceListItem.TextContent.Should().Be("0.0 miles");

            var qualificationList = searchResults.Children[itemNumber].QuerySelector("#tl-qualification-list");
            var shortTitleListItem = qualificationList.Children[0] as IHtmlListItemElement;
            shortTitleListItem.TextContent.Should().Be("Short Title");
        }
    }
}