using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Opportunity_Basket_Page_Multiple_Referral_And_Provision_Gap_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1050;
        private const int OpportunityItem1Id = 1051;
        private const int OpportunityItem2Id = 1052;
        private const int OpportunityItem3Id = 1053;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Opportunity_Basket_Page_Multiple_Referral_And_Provision_Gap_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
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

            var tabList = documentHtml.QuerySelector(".govuk-tabs__list");
            var providerTab = tabList.Children[0] as IHtmlListItemElement;
            providerTab.Text().Should().Be("\n            \n                With providers\n            \n        ");

            var noProviderTab = tabList.Children[1] as IHtmlListItemElement;
            noProviderTab.Text().Should().Be("\n            \n                With no providers\n            \n        ");

            var providerBasketTable = documentHtml.QuerySelector("#providers .govuk-table") as IHtmlTableElement;
            providerBasketTable.Head.Rows.Length.Should().Be(1);
            providerBasketTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Workplace");
            providerBasketTable.Head.Rows[0].Cells[2].TextContent.Should().Be("Job role");
            providerBasketTable.Head.Rows[0].Cells[3].TextContent.Should().Be("Students wanted");
            providerBasketTable.Head.Rows[0].Cells[4].TextContent.Should().Be("Providers");

            var providerBasketTableBody = providerBasketTable.Bodies[0];
            providerBasketTableBody.Rows.Length.Should().Be(2);

            AssertTableRow(providerBasketTableBody.Rows[0],
                "London SW1A 2AA",
                "Job Role",
                "1",
                "1", 
                OpportunityItem1Id);

            AssertTableRow(providerBasketTableBody.Rows[1],
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

            var noProviderBasketTable = documentHtml.QuerySelector("#no-providers .govuk-table") as IHtmlTableElement;
            noProviderBasketTable.Head.Rows.Length.Should().Be(1);
            noProviderBasketTable.Head.Rows[0].Cells[0].TextContent.Should().Be("Workplace");
            noProviderBasketTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Job role");
            noProviderBasketTable.Head.Rows[0].Cells[2].TextContent.Should().Be("Students wanted");
            noProviderBasketTable.Head.Rows[0].Cells[3].TextContent.Should().Be("Reason no providers chosen");

            var noProviderRow = noProviderBasketTable.Rows[1];
            noProviderRow.Cells[0].TextContent.Should().Be("London SW1A 2AA");
            noProviderRow.Cells[1].TextContent.Should().Be("Job Role");
            noProviderRow.Cells[2].TextContent.Should().Be("1");
            noProviderRow.Cells[3].TextContent.Should().Be("Employer had a bad experience with them");

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