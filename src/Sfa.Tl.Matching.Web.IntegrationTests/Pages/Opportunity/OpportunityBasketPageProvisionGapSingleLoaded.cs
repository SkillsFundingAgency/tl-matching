using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class OpportunityBasketPageProvisionGapSingleLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1020;
        private const int OpportunityItemId = 1021;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public OpportunityBasketPageProvisionGapSingleLoaded(CustomWebApplicationFactory<TestStartup> factory)
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
            var row = basketTable.Rows[1];
            row.Cells[0].TextContent.Should().Be("London SW1A 2AA");
            row.Cells[1].TextContent.Should().Be("Job Role");
            row.Cells[2].TextContent.Should().Be("1");
            row.Cells[3].TextContent.Should().Be("Employer had a bad experience with them");

            var addAnotherLink = documentHtml.QuerySelector("#tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var finishButton = documentHtml.QuerySelector("#tl-finish") as IHtmlButtonElement;
            finishButton.Name.Should().Be("SubmitAction");
            finishButton.Value.Should().Be("CompleteProvisionGaps");
            finishButton.TextContent.Should().Be("Finish");

            var downloadLink = documentHtml.QuerySelector("#tl-download") as IHtmlAnchorElement;
            downloadLink.Text.Should().Be("Download all data as a spreadsheet");
            downloadLink.PathName.Should().Be($"/download-opportunity/{OpportunityId}");
        }
    }
}