using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Saved_Opportunities_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Saved opportunities";
        private const int OpportunityId = 1010;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Saved_Opportunities_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"saved-opportunities");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Finish and return to start");
            backLink.PathName.Should().Be($"/Start");

            var table = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            table.Rows.Length.Should().Be(6);

            AssertTableRow(table.Rows[1], "Company Name",
                "12:00am on 01 January 2019", 1010);

            AssertTableRow(table.Rows[2], "Company Name",
                "12:00am on 04 January 2019", 1020);

            AssertTableRow(table.Rows[3], "Company Name",
                "12:00am on 02 January 2019", 1030);

            AssertTableRow(table.Rows[4], "Company Name",
                "11:59pm on 07 April 2019", 1050);

            AssertTableRow(table.Rows[5], "Company Name",
                "04:22pm on 05 November 2018", 1040);
        }

        private static void AssertTableRow(IHtmlTableRowElement row, string employerName, string lastUpdated, int opportunityId)
        {
            row.Cells[0].TextContent.Should().Be(employerName);
            row.Cells[1].TextContent.Should().Be(lastUpdated);

            var rowEditCell = row.Cells[2].Children[0] as IHtmlAnchorElement;
            rowEditCell.Text.Should().Be("Edit this employer’s opportunities");
            rowEditCell.PathName.Should().Be($"/employer-opportunities/{opportunityId}-0");

            var rowDeleteCell = row.Cells[3].Children[0] as IHtmlAnchorElement;
            rowDeleteCell.Text.Should().Be("Delete this employer’s opportunities");
            rowDeleteCell.PathName.Should().Be($"/confirm-remove-employer/{opportunityId}");
        }
    }
}