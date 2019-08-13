﻿using System.Threading.Tasks;
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
            //basketTable.Rows.Length.Should().Be(2);

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