using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Check_Answers_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Check answers";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Check_Answers_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"check-answers/{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be($"/get-placement-or-employer/{OpportunityId}-{OpportunityItemId}");

            var cancelLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            cancelLink.PathName.Should().Be($"/remove-opportunityItem/{OpportunityId}-{OpportunityItemId}");

            var providerResultsUrl =
                $"/provider-results-for-opportunity-1000-item-{OpportunityItemId}-within-10-miles-of-SW1A%202AA-for-route-1";

            var placementInformationTable = documentHtml.GetElementById("tl-placement-table") as IHtmlTableElement;

            AssertPlacementTableRow(placementInformationTable.Rows[0],
                "Agriculture, environmental and animal care",
                "Change the type of placement",
                providerResultsUrl);

            AssertPlacementTableRow(placementInformationTable.Rows[1],
                "SW1A 2AA",
                "Change the postcode of the workplace",
                providerResultsUrl);

            AssertPlacementTableRow(placementInformationTable.Rows[2],
                "Job Role",
                "Change the job role",
                $"/placement-information/{OpportunityItemId}");

            AssertPlacementTableRow(placementInformationTable.Rows[3],
                "1",
                "Change the number of placements",
                $"/placement-information/{OpportunityItemId}");

            var providerTable = documentHtml.GetElementById("tl-providers-table") as IHtmlTableElement;
            var provider1Row = providerTable.Rows[0];
            var providerNameCell = provider1Row.Cells[0] as IHtmlTableHeaderCellElement;
            providerNameCell.TextContent.Should().Be(" (part of SQL Search Provider Display Name)");

            var distanceCell = provider1Row.Cells[1] as IHtmlTableDataCellElement;
            distanceCell.TextContent.Should()
                .Be("\n                            1.2 miles from SW1A 2AA\n                        ");

            var changeProvidersLink = documentHtml.GetElementById("tl-change-providers") as IHtmlAnchorElement;
            changeProvidersLink.TextContent.Should().Be("Change providers");
            changeProvidersLink.PathName.Should().Be(providerResultsUrl);

            var confirmButton = documentHtml.GetElementById("tl-confirm") as IHtmlButtonElement;
            confirmButton.TextContent.Should().Be("Confirm and save opportunity");
            confirmButton.Type.Should().Be("submit");
        }

        private static void AssertPlacementTableRow(IHtmlTableRowElement row, string cell1Text, string cell2Text,
            string cell2Path)
        {
            row.Cells[1].TextContent.Should().Be(cell1Text);
            var changeCell = row.Cells[2].Children[0] as IHtmlAnchorElement;
            changeCell.Text().Should().Be(cell2Text);
            changeCell.PathName.Should().Be(cell2Path);
        }
    }
}