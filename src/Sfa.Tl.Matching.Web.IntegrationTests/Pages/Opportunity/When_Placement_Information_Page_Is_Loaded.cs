using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Placement_Information_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private const string Title = "Placement information";
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        public When_Placement_Information_Page_Is_Loaded(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"placement-information/{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var jobRole = documentHtml.GetElementById("JobRole") as IHtmlInputElement;
            jobRole.Value.Should().Be("Job Role");

            var placements = documentHtml.GetElementById("Placements") as IHtmlInputElement;
            placements.Value.Should().Be("1");

            var placementLocationYes = documentHtml.GetElementById("placement-location-yes") as IHtmlInputElement;
            placementLocationYes.Value.Should().Be("true");

            var placementLocationNo = documentHtml.GetElementById("placement-location-no") as IHtmlInputElement;
            placementLocationNo.Value.Should().Be("false");

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue");
            continueButton.Type.Should().Be("submit");
        }
    }
}