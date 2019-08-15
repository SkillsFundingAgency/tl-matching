using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Opportunity_Basket_Page_Referral_Multiple_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1030;
        private const int OpportunityItem1Id = 1031;
        private const int OpportunityItem2Id = 1032;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Opportunity_Basket_Page_Referral_Multiple_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"employer-opportunities/{OpportunityId}-{OpportunityItem1Id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var saveLink = documentHtml.GetElementById("tl-save") as IHtmlAnchorElement;
            saveLink.Text.Should().Be("Save and come back later");
            saveLink.PathName.Should().Be($"/saved-opportunities");

            var basketTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            basketTable.Head.Rows.Length.Should().Be(1);
            basketTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Workplace");
            basketTable.Head.Rows[0].Cells[2].TextContent.Should().Be("Job role");
            basketTable.Head.Rows[0].Cells[3].TextContent.Should().Be("Students wanted");
            basketTable.Head.Rows[0].Cells[4].TextContent.Should().Be("Providers");

            var basketTableBody = basketTable.Bodies[0];
            basketTableBody.Rows.Length.Should().Be(2);

            AssertTableRow(basketTableBody.Rows[0],
                "London SW1A 2AA",
                "Job Role",
                "1",
                "1",
                OpportunityItem1Id);

            AssertTableRow(basketTableBody.Rows[1],
                "London SW2A 3AA",
                "Job Role",
                "1",
                "1",
                OpportunityItem2Id);

            var addAnotherLink = documentHtml.GetElementById("tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.Name.Should().Be("SubmitAction");
            continueButton.Value.Should().Be("SaveSelectedOpportunities");
            continueButton.TextContent.Should().Be("Continue with selected opportunities");

            var downloadLink = documentHtml.GetElementById("tl-download") as IHtmlAnchorElement;
            downloadLink.Text.Should().Be("Download all data as a spreadsheet");
            downloadLink.PathName.Should().Be($"/download-opportunity/{OpportunityId}");
        }

        private static void AssertTableRow(IHtmlTableRowElement row, string workplace, string jobRole, string studentsWanted,
            string providers, int opportunityItemId)
        {
            row.Cells[1].TextContent.Should().Be(workplace);
            row.Cells[2].TextContent.Should().Be(jobRole);
            row.Cells[3].TextContent.Should().Be(studentsWanted);
            row.Cells[4].TextContent.Should().Be(providers);

            var editCell = row.Cells[5].Children[0] as IHtmlAnchorElement;
            editCell.Text.Should().Be("Edit");
            editCell.PathName.Should().Be($"/check-answers/{opportunityItemId}");

            var deleteCell = row.Cells[6].Children[0] as IHtmlAnchorElement;
            deleteCell.Text.Should().Be("Delete");
            deleteCell.PathName.Should().Be($"/remove-opportunity/{opportunityItemId}");
        }
    }
}