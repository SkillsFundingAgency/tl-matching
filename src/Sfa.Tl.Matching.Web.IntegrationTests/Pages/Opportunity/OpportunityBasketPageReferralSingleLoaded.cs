using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class OpportunityBasketPageReferralSingleLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1010;
        private const int OpportunityItemId = 1011;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public OpportunityBasketPageReferralSingleLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"employer-opportunities/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var basketTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            var row = basketTable.Rows[1];
            row.Cells[0].TextContent.Should().Be("London SW1A 2AA");
            row.Cells[1].TextContent.Should().Be("Job Role");
            row.Cells[2].TextContent.Should().Be("1");
            row.Cells[3].TextContent.Should().Be("1");

            var editCell = row.Cells[4].Children[0] as IHtmlAnchorElement;
            editCell.Text.Should().Be("Edit");
            editCell.PathName.Should().Be($"/check-answers/{OpportunityItemId}");

            var deleteCell = row.Cells[5].Children[0] as IHtmlAnchorElement;
            deleteCell.Text.Should().Be("Delete");
            deleteCell.PathName.Should().Be($"/remove-opportunity/{OpportunityItemId}");

            var addAnotherLink = documentHtml.QuerySelector("#tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var continueButton = documentHtml.QuerySelector("#tl-continue") as IHtmlButtonElement;
            continueButton.Name.Should().Be("SubmitAction");
            continueButton.Value.Should().Be("SaveSelectedOpportunities");
            continueButton.TextContent.Should().Be("Continue with opportunity");
        }
    }
}