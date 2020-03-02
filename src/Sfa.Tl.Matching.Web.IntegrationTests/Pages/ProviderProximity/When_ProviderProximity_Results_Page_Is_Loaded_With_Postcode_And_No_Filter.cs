using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.ProviderProximity
{
    public class When_ProviderProximity_Results_Page_Is_Loaded_With_Postcode_And_No_Filter : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All providers in an area";

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_ProviderProximity_Results_Page_Is_Loaded_With_Postcode_And_No_Filter(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/all-provider-results-CV12WT");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be("Providers within 30 miles of CV12WT");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/find-all-providers");

            var searchCount = documentHtml.GetElementById("tl-search-count");
            searchCount.TextContent.Should().Be("2");

            var allFilterCheckboxes =
                documentHtml.QuerySelectorAll(".govuk-checkboxes__input") as IHtmlCollection<IElement>;
            foreach (var filterCheckbox in allFilterCheckboxes)
            {
                var htmlInput = filterCheckbox as IHtmlInputElement;
                htmlInput.IsChecked.Should().BeFalse();
            }

            var finishLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            finishLink.PathName.Should().Be("/Start");

            var filterButton = documentHtml.GetElementById("tl-filter") as IHtmlButtonElement;
            filterButton.TextContent.Should().Be("Filter results");
            filterButton.Type.Should().Be("submit");

            var filterRemove = documentHtml.GetElementById("tl-filter-remove") as IHtmlAnchorElement;
            filterRemove.Should().BeNull();

            var searchResults = documentHtml.QuerySelector(".tl-search-results") as IHtmlDivElement;
            AssertSearchResult(searchResults);
        }

        private void AssertSearchResult(IHtmlDivElement searchResults)
        {
            var tLevelProvider = searchResults.QuerySelector("#tl-provider");
            tLevelProvider.TextContent.Should().Be("T level provider");
            tLevelProvider.ClassName.Should().Be("tl-search-results-flag");

            var venueDetailList = searchResults.QuerySelector("#tl-venue-detail-list");
            var providerNameListItem = venueDetailList.Children[0] as IHtmlListItemElement;
            providerNameListItem.TextContent.Should().Be("Part of SQL Search Provider Display Name");

            var townAndPostcodeListItem = venueDetailList.Children[1] as IHtmlListItemElement;
            townAndPostcodeListItem.TextContent.Should().Be("Coventry CV1 2WT");

            var journeyInfo = searchResults.QuerySelector("#tl-journey-info");
            journeyInfo.Children.Length.Should().Be(1);

            var distanceItem = journeyInfo.Children[0] as IHtmlParagraphElement;
            distanceItem.TextContent.Should().Be("0.0 miles");

            var qualificationList = searchResults.QuerySelector("#tl-qualification-list") as IHtmlUnorderedListElement;
            var routeName1 = qualificationList.Children[0] as IHtmlListItemElement;
            routeName1.QuerySelector("details > summary > span").TextContent.Should().Be("Agriculture, environmental and animal care");

            var shortTitleList = routeName1.QuerySelector("details > div > ul") as IHtmlUnorderedListElement;
            var shortTitle1 = shortTitleList.Children[0] as IHtmlListItemElement;
            shortTitle1.TextContent.Should().Be("Short Title");
        }
    }
}