using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class OpportunityBasketPageReferralMultipleLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1030;
        private const int OpportunityItemId = 1031;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public OpportunityBasketPageReferralMultipleLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            // ReSharper disable all PossibleNullReferenceException

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

            var saveLink = documentHtml.QuerySelector("#tl-save") as IHtmlAnchorElement;
            saveLink.Text.Should().Be("Save and come back later");
            saveLink.PathName.Should().Be($"/saved-opportunities");

            var basketTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            var row1 = basketTable.Rows[1];
            row1.Cells[1].TextContent.Should().Be("London SW1A 2AA");
            row1.Cells[2].TextContent.Should().Be("Job Role");
            row1.Cells[3].TextContent.Should().Be("1");
            row1.Cells[4].TextContent.Should().Be("1");
            var editRow1Cell = row1.Cells[5].Children[0] as IHtmlAnchorElement;
            editRow1Cell.Text.Should().Be("Edit");
            editRow1Cell.PathName.Should().Be($"/check-answers/{OpportunityItemId}");
            var deleteRow1Cell = row1.Cells[6].Children[0] as IHtmlAnchorElement;
            deleteRow1Cell.Text.Should().Be("Delete");
            deleteRow1Cell.PathName.Should().Be($"/remove-opportunity/{OpportunityItemId}");

            var row2 = basketTable.Rows[2];
            row2.Cells[1].TextContent.Should().Be("London SW2A 3AA");
            row2.Cells[2].TextContent.Should().Be("Job Role");
            row2.Cells[3].TextContent.Should().Be("1");
            row2.Cells[4].TextContent.Should().Be("1");
            var editRow2Cell = row1.Cells[5].Children[0] as IHtmlAnchorElement;
            editRow2Cell.Text.Should().Be("Edit");
            editRow2Cell.PathName.Should().Be($"/check-answers/{OpportunityItemId}");
            var deleteRow2Cell = row1.Cells[6].Children[0] as IHtmlAnchorElement;
            deleteRow2Cell.Text.Should().Be("Delete");
            deleteRow2Cell.PathName.Should().Be($"/remove-opportunity/{OpportunityItemId}");
            
            var addAnotherLink = documentHtml.QuerySelector("#tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var continueButton = documentHtml.QuerySelector("#tl-continue") as IHtmlButtonElement;
            continueButton.Name.Should().Be("SubmitAction");
            continueButton.Value.Should().Be("SaveSelectedOpportunities");
            continueButton.TextContent.Should().Be("Continue with selected opportunities");

            var downloadLink = documentHtml.QuerySelector("#tl-download") as IHtmlAnchorElement;
            downloadLink.Text.Should().Be("Download all data as a spreadsheet");
            downloadLink.PathName.Should().Be($"/download-opportunity/{OpportunityId}");
        }
    }
}