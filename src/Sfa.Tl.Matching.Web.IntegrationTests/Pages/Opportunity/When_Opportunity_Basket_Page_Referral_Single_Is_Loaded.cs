using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Opportunity_Basket_Page_Referral_Single_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1010;
        private const int OpportunityItemId = 1011;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Opportunity_Basket_Page_Referral_Single_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
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

            var saveLink = documentHtml.GetElementById("tl-save") as IHtmlAnchorElement;
            saveLink.Text.Should().Be("Save and come back later");
            saveLink.PathName.Should().Be($"/saved-opportunities");

            var basketTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            basketTable.Head.Rows.Length.Should().Be(1);
            basketTable.Head.Rows[0].Cells[0].TextContent.Should().Be("Workplace");
            basketTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Job role");
            basketTable.Head.Rows[0].Cells[2].TextContent.Should().Be("Students wanted");
            basketTable.Head.Rows[0].Cells[3].TextContent.Should().Be("Providers");

            var basketTableBody = basketTable.Bodies[0];
            basketTableBody.Rows.Length.Should().Be(1);
            basketTableBody.Rows[0].Cells[0].TextContent.Should().Be("London SW1A 2AA");
            basketTableBody.Rows[0].Cells[1].TextContent.Should().Be("Job Role");
            basketTableBody.Rows[0].Cells[2].TextContent.Should().Be("1");
            basketTableBody.Rows[0].Cells[3].TextContent.Should().Be("Venue 1 Name (part of SQL Search Provider Display Name)");

            var editCell = basketTableBody.Rows[0].Cells[4].Children[0] as IHtmlAnchorElement;
            editCell.Text.Should().Be("Edit");
            editCell.PathName.Should().Be($"/check-answers/{OpportunityItemId}");

            var deleteCell = basketTableBody.Rows[0].Cells[5].Children[0] as IHtmlAnchorElement;
            deleteCell.Text.Should().Be("Delete");
            deleteCell.PathName.Should().Be($"/remove-opportunity/{OpportunityItemId}");

            var addAnotherLink = documentHtml.GetElementById("tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.Name.Should().Be("SubmitAction");
            continueButton.Value.Should().Be("SaveSelectedOpportunities");
            continueButton.TextContent.Should().Be("Continue with opportunity");

            var downloadLink = documentHtml.GetElementById("tl-download") as IHtmlAnchorElement;
            downloadLink.Text.Should().Be("Download all data as a spreadsheet");
            downloadLink.PathName.Should().Be($"/download-opportunity/{OpportunityId}");
        }
    }
}