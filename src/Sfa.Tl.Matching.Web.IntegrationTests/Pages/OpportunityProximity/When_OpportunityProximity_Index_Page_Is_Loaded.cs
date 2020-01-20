using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.OpportunityProximity
{
    public class When_OpportunityProximity_Index_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private const string Title = "Set up placement opportunity";
        private const int OpportunityId = 0;
        private const int OpportunityItemId = 0;

        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        public When_OpportunityProximity_Index_Page_Is_Loaded(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/find-providers");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var cancelLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            cancelLink.PathName.Should().Be($"/remove-opportunityItem/{OpportunityId}-{OpportunityItemId}");

            var routeList = documentHtml.GetElementById("SelectedRouteId");
            routeList.Children.Length.Should().Be(2);
            routeList.Text().Should().Be("Agriculture, environmental and animal care\nBusiness and administration\n");

            var postcode = documentHtml.GetElementById("Postcode");
            postcode.Should().NotBeNull();

            var search = documentHtml.GetElementById("tl-search") as IHtmlButtonElement;
            search.TextContent.Should().Be("Search");
            search.Type.Should().Be("submit");
        }
    }
}