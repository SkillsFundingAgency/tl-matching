using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.ProviderVenue
{
    public class When_Venue_Overview_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "CV1 2WT";
        private const int VenueId = 1;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Venue_Overview_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/venue-overview/{VenueId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/get-admin-back-link/1");

            var searchVisibleYes = documentHtml.GetElementById("search-visible-yes") as IHtmlInputElement;
            searchVisibleYes.Value.Should().Be("true");
            var searchVisibleNo = documentHtml.GetElementById("search-visible-no") as IHtmlInputElement;
            searchVisibleNo.Value.Should().Be("false");

            var providerId = documentHtml.GetElementById("ProviderId") as IHtmlInputElement;
            providerId.Type.Should().Be("hidden");
            providerId.Value.Should().Be("1");

            var source = documentHtml.GetElementById("Source") as IHtmlInputElement;
            source.Type.Should().Be("hidden");
            source.Value.Should().Be("Admin");

            var name = documentHtml.GetElementById("Name") as IHtmlInputElement;
            name.Value.Should().Be("Venue 1 Name");

            var removeLink = documentHtml.GetElementById("tl-remove") as IHtmlAnchorElement;
            removeLink.Text.Should().Be("Remove venue");
            removeLink.PathName.Should().Be("/remove-venue/1");

            var saveSectionButton = documentHtml.GetElementById("tl-save-section") as IHtmlButtonElement;
            saveSectionButton.TextContent.Should().Be("Save section");
            saveSectionButton.Type.Should().Be("submit");
            saveSectionButton.Name.Should().Be("SubmitAction");
            saveSectionButton.Value.Should().Be("SaveSection");

            var qualificationTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            qualificationTable.Head.Rows.Length.Should().Be(1);
            qualificationTable.Head.Rows[0].Cells[0].TextContent.Should().Be("Learning aim reference");
            qualificationTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Qualification title");
            qualificationTable.Head.Rows[0].Cells[2].TextContent.Should().BeEmpty();

            var qualificationTableBody = qualificationTable.Bodies[0];
            qualificationTableBody.Rows.Length.Should().Be(1);

            AssertTableRow(qualificationTableBody.Rows[0],
                "12345678",
                "Qualification Title",
                1,
                1);

            var addQualificationLink = documentHtml.GetElementById("tl-add-qualification") as IHtmlAnchorElement;
            addQualificationLink.Text.Should().Be("Add a qualification");
            addQualificationLink.PathName.Should().Be($"/add-qualification/{VenueId}");

            var saveAndFinishButton = documentHtml.GetElementById("tl-confirm") as IHtmlButtonElement;
            saveAndFinishButton.TextContent.Should().Be("Save and return to provider overview");
            saveAndFinishButton.Type.Should().Be("submit");
            saveAndFinishButton.Name.Should().Be("SubmitAction");
            saveAndFinishButton.Value.Should().Be("SaveAndFinish");
        }

        private static void AssertTableRow(IHtmlTableRowElement row, string larId, string qualificationTitle,
            int qualificationId, int venueId)
        {
            row.Cells[0].TextContent.Should().Be(larId);
            row.Cells[1].TextContent.Should().Be(qualificationTitle);

            var deleteCell = row.Cells[2].Children[0] as IHtmlAnchorElement;
            deleteCell.TextContent.Should().Be("Remove qualification");
            deleteCell.PathName.Should().Be($"/remove-qualification/{venueId}/{qualificationId}");
        }
    }
}