using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class CheckAnswersPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Check answers";
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public CheckAnswersPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"check-answers/{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var indexViewHtml = await HtmlHelpers.GetDocumentAsync(response);

            indexViewHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = indexViewHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = indexViewHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var providerResultsUrl =
                $"/provider-results-for-opportunity-1000-item-{OpportunityItemId}-within-10-miles-of-SW1A%202AA-for-route-1";

            var placementInformationTable = indexViewHtml.QuerySelector(".govuk-table") as IHtmlTableElement;
            var skillAreaRow = placementInformationTable.Rows[0];
            skillAreaRow.Cells[1].TextContent.Should().Be("\n                        Agriculture, environmental and animal care\n                    ");
            var skillAreaChangeCell = skillAreaRow.Cells[2].Children[0] as IHtmlAnchorElement;
            skillAreaChangeCell.Text().Should().Be("Change the type of placement");
            skillAreaChangeCell.PathName.Should().Be(providerResultsUrl);

            var postcodeRow = placementInformationTable.Rows[1];
            postcodeRow.Cells[1].TextContent.Should().Be("\n                        SW1A 2AA\n                    ");
            var postcodeChangeCell = postcodeRow.Cells[2].Children[0] as IHtmlAnchorElement;
            postcodeChangeCell.Text().Should().Be("Change the postcode of the workplace");
            postcodeChangeCell.PathName.Should().Be(providerResultsUrl);

            var jobRoleRow = placementInformationTable.Rows[2];
            jobRoleRow.Cells[1].TextContent.Should().Be("\n                        Job Role\n                    ");
            var jobRoleChangeCell = jobRoleRow.Cells[2].Children[0] as IHtmlAnchorElement;
            jobRoleChangeCell.Text().Should().Be("Change the job role");
            jobRoleChangeCell.PathName.Should().Be($"/placement-information/{OpportunityItemId}");

            var studentsWantedRow = placementInformationTable.Rows[3];
            studentsWantedRow.Cells[1].TextContent.Should().Be("\n                        1\n                    ");
            var studentsWantedChangeCell = studentsWantedRow.Cells[2].Children[0] as IHtmlAnchorElement;
            studentsWantedChangeCell.Text().Should().Be("Change the number of placements");
            studentsWantedChangeCell.PathName.Should().Be($"/placement-information/{OpportunityItemId}");

            // TODO Assert Provider Information -- Change UI

            var changeProvidersLink = indexViewHtml.QuerySelector("#tl-change-providers") as IHtmlAnchorElement;
            changeProvidersLink.TextContent.Should().Be("Change providers");
            changeProvidersLink.PathName.Should().Be(providerResultsUrl);
        }
    }
}