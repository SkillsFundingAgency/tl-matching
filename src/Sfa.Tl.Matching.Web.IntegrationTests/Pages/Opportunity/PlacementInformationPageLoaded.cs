using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class PlacementInformationPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Placement information";
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public PlacementInformationPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"placement-information/{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var jobRole = documentHtml.QuerySelector("#JobRole") as IHtmlInputElement;
            jobRole.Value.Should().Be("Job Role");

            var placements = documentHtml.QuerySelector("#Placements") as IHtmlInputElement;
            placements.Value.Should().Be("1");

            var placementLocationYes = documentHtml.QuerySelector("#placement-location-yes") as IHtmlInputElement;
            placementLocationYes.Value.Should().Be("true");

            var placementLocationNo = documentHtml.QuerySelector("#placement-location-no") as IHtmlInputElement;
            placementLocationNo.Value.Should().Be("false");

            var continueButton = documentHtml.QuerySelector("#tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue");
        }
    }
}