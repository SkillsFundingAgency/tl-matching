using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.ProviderProximity
{
    public class When_ProviderProximity_Results_Page_Is_Loaded_With_Postcode : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All providers in an area";

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_ProviderProximity_Results_Page_Is_Loaded_With_Postcode(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/provider-results-cv12wt");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be("Providers within 30 miles of CV12WT");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/Start");

            var searchCount = documentHtml.GetElementById("tl-search-count");
            searchCount.TextContent.Should().Be("8");

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


            // TODO AU Add search results
        }
    }
}