using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Provider_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Select providers for this opportunity";
        private const int OpportunityId = 0;
        private const int OpportunityItemId = 0;
        private const int SearchRadius = 25;
        private const string Postcode = "CV1 2WT";
        private const int SelectedRouteId = 1;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Provider_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var resultText = documentHtml.QuerySelector(".govuk-body"); 
            resultText.TextContent.Trim().Should().Be("2 results in Agriculture, environmental and animal care within 25 miles of CV1 2WT");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            //var searchResult = documentHtml.GetElementsByClassName("tl-inline-list") as IHtmlCollection<IElement>;

            //var textContents = searchResult.Select(x => x.TextContent.Trim());
            //textContents.Should().Contain("Part of SQL Search Provider Display Name");
            //textContents.Should().Contain("Coventry CV1 2WT");

            var submitButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            submitButton.TextContent.Should().Be("Continue with selected providers");
        }
    }
}
