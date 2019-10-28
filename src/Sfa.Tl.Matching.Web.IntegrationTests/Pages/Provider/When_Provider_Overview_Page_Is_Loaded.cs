using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Provider_Overview_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "SQL Search Provider";
        private const int ProviderId = 1;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Provider_Overview_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/provider-overview/{ProviderId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should()
                .Be("\n                UKPRN: 10203040\n                SQL Search Provider\n            ");
            header1.Children[0].TextContent.Should().Be("UKPRN: 10203040");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/search-ukprn");

            var cdfYes = documentHtml.GetElementById("cdf-yes") as IHtmlInputElement;
            cdfYes.Value.Should().Be("true");
            var cdfNo = documentHtml.GetElementById("cdf-no") as IHtmlInputElement;
            cdfNo.Value.Should().Be("false");

            var saveSectionButton = documentHtml.GetElementById("tl-save-section") as IHtmlButtonElement;
            saveSectionButton.TextContent.Should().Be("Save");
            saveSectionButton.Type.Should().Be("submit");
            saveSectionButton.Name.Should().Be("SubmitAction");
            saveSectionButton.Value.Should().Be("SaveSection");

            var placementsYes = documentHtml.GetElementById("placements-yes") as IHtmlInputElement;
            placementsYes.Value.Should().Be("true");
            var placementsNo = documentHtml.GetElementById("placements-no") as IHtmlInputElement;
            placementsNo.Value.Should().Be("false");

            var tLevelYes = documentHtml.GetElementById("tlevel-yes") as IHtmlInputElement;
            tLevelYes.Value.Should().Be("true");
            var tLevelNo = documentHtml.GetElementById("tlevel-no") as IHtmlInputElement;
            tLevelNo.Value.Should().Be("false");

            var displayName = documentHtml.GetElementById("DisplayName") as IHtmlInputElement;
            displayName.Value.Should().Be("SQL Search Provider Display Name");

            var primaryContact = documentHtml.GetElementById("PrimaryContact") as IHtmlInputElement;
            primaryContact.Value.Should().Be("Test");

            var primaryContactEmail = documentHtml.GetElementById("PrimaryContactEmail") as IHtmlInputElement;
            primaryContactEmail.Value.Should().Be("Test@test.com");

            var primaryContactPhone = documentHtml.GetElementById("PrimaryContactPhone") as IHtmlInputElement;
            primaryContactPhone.Value.Should().Be("0123456789");

            var secondaryContact = documentHtml.GetElementById("SecondaryContact") as IHtmlInputElement;
            secondaryContact.Value.Should().Be("Test 2");

            var secondaryContactEmail = documentHtml.GetElementById("SecondaryContactEmail") as IHtmlInputElement;
            secondaryContactEmail.Value.Should().Be("Test2@test.com");

            var secondaryContactPhone = documentHtml.GetElementById("SecondaryContactPhone") as IHtmlInputElement;
            secondaryContactPhone.Value.Should().Be("0987654321");

            var venueTable = documentHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            venueTable.Head.Rows.Length.Should().Be(1);
            venueTable.Head.Rows[0].Cells[0].TextContent.Should().Be("Postcode");
            venueTable.Head.Rows[0].Cells[1].TextContent.Should().Be("Number of qualifications");
            venueTable.Head.Rows[0].Cells[2].TextContent.Should().Be("Help finding placements");
            venueTable.Head.Rows[0].Cells[3].TextContent.Should().BeEmpty();

            var venueTableBody = venueTable.Bodies[0];
            venueTableBody.Rows.Length.Should().Be(2);

            AssertTableRow(venueTableBody.Rows[0],
                "CV1 1EE",
                "1",
                "Yes",
                2,
                1);

            AssertTableRow(venueTableBody.Rows[1],
                "CV1 2WT",
                "1",
                "Yes",
                1,
                1);

            var addVenueLink = documentHtml.GetElementById("tl-add-venue") as IHtmlAnchorElement;
            addVenueLink.Text.Should().Be("Add a venue");
            addVenueLink.PathName.Should().Be($"/add-venue/{ProviderId}");

            var saveAndFinishButton = documentHtml.GetElementById("tl-save-finish") as IHtmlButtonElement;
            saveAndFinishButton.TextContent.Should().Be("Save and finish");
            saveAndFinishButton.Type.Should().Be("submit");
            saveAndFinishButton.Name.Should().Be("SubmitAction");
            saveAndFinishButton.Value.Should().Be("SaveAndFinish");
        }

        private static void AssertTableRow(IHtmlTableRowElement row, string postcode, string numberOfQualifications, string helpFindingPlacements, 
            int venueId, int providerId)
        {
            row.Cells[0].TextContent.Should().Be(postcode);
            row.Cells[1].TextContent.Should().Be(numberOfQualifications);
            row.Cells[2].TextContent.Should().Be(helpFindingPlacements);

            var editCell = row.Cells[3].Children[0] as IHtmlAnchorElement;
            editCell.TextContent.Should().Be("Edit this venue");
            editCell.PathName.Should().Be($"/venue-overview/{venueId}");
            editCell.Search.Should().Be($"?providerId={providerId}");
        }
    }
}