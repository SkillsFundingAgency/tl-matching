using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Provider_Search_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Find a provider";
        private const int OpportunityId = 0;
        private const int OpportunityItemId = 0;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Provider_Search_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/search-ukprn");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/Start");

            var search = documentHtml.GetElementById("tl-search") as IHtmlButtonElement;
            search.TextContent.Should().Be("Search");
            search.Type.Should().Be("submit");

            var providerTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            providerTable.Head.Rows.Length.Should().Be(1);
            providerTable.Head.Rows[0].Cells[0].TextContent.Should().Be("UKPRN");
            providerTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Provider name");
            providerTable.Head.Rows[0].Cells[2].TextContent.Should().Be("CDF provider");

            var providerTableBody = providerTable.Bodies[0];
            providerTableBody.Rows.Length.Should().Be(1);

            AssertTableRow(providerTableBody.Rows[0],
                "10203040",
                "SQL Search Provider",
                "Yes",
                1);
        }

        private static void AssertTableRow(IHtmlTableRowElement row, string ukprn, string name, string cdfProvider, 
            int providerId)
        {
            row.Cells[0].TextContent.Should().Be(ukprn);
            row.Cells[1].TextContent.Should().Be(name);
            row.Cells[2].TextContent.Should().Be(cdfProvider);

            var editCell = row.Cells[3].Children[0] as IHtmlAnchorElement;
            editCell.TextContent.Should().Be("Edit SQL Search Provider's details");
            editCell.PathName.Should().Be($"/provider-overview/{providerId}");
        }
    }
}