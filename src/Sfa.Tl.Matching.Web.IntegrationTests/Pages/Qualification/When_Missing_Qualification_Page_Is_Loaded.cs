using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Qualification
{
    public class When_Missing_Qualification_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private const string Title = "We need extra information for this qualification";
        private const int VenueId = 1;
        private const string LarId = "12345678";

        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        public When_Missing_Qualification_Page_Is_Loaded(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/missing-qualification/{VenueId}/{LarId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/get-admin-back-link/1");

            var larId = documentHtml.GetElementById("tl-qualification-larid") as IHtmlParagraphElement;
            larId.TextContent.Should().Be("12345678");

            var title = documentHtml.GetElementById("tl-qualification-title") as IHtmlParagraphElement;
            title.TextContent.Should().Be("Qualification Title");

            var changeLink = documentHtml.GetElementById("tl-change") as IHtmlAnchorElement;
            changeLink.Text.Should().Be("Change qualification");
            changeLink.PathName.Should().Be($"/add-qualification/{VenueId}");

            var shortTitle = documentHtml.GetElementById("shortTitleHidden") as IHtmlInputElement;
            shortTitle.Value.Should().BeEmpty("Short Title");

            var skillAreas = documentHtml.QuerySelectorAll(".govuk-checkboxes__item");

            AssertSkillArea(skillAreas, 
                0, 
                1, 
                "Agriculture, environmental and animal care",
                "Includes: animal care and management; agriculture, land management and production");

            AssertSkillArea(skillAreas,
                1,
                2,
                "Business and administration",
                "Includes: management and administration; human resources (HR)");

            var continueButton = documentHtml.GetElementById("tl-add") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Add qualification");
            continueButton.Type.Should().Be("submit");
        }

        private void AssertSkillArea(IHtmlCollection<IElement> skillAreas, int itemNumber, int routeIdValue, 
            string routeLabelValue, string routeSummaryValue)
        {
            var routeId = skillAreas[itemNumber].QuerySelector("#route_Id") as IHtmlInputElement;
            routeId.Type.Should().Be("hidden");
            routeId.Value.Should().Be(routeIdValue.ToString());

            var routeIsSelected = skillAreas[itemNumber].QuerySelector($"#Routes_{itemNumber}__IsSelected") as IHtmlInputElement;
            routeIsSelected.Type.Should().Be("checkbox");
            routeIsSelected.Value.Should().Be("true");

            var routeLabel = skillAreas[itemNumber].QuerySelector($".govuk-label") as IHtmlLabelElement;
            routeLabel.TextContent.Should().Be(routeLabelValue);

            var summarySpan = skillAreas[itemNumber].QuerySelector(".govuk-hint") as IHtmlSpanElement;
            summarySpan.TextContent.Should().Be(routeSummaryValue);
        }
    }
}