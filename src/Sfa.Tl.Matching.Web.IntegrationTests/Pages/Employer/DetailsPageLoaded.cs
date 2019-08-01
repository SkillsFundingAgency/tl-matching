using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class DetailsPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Confirm contact details for industry placements";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public DetailsPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"employer-details/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var findAnotherLink = documentHtml.QuerySelector("#tl-find-different") as IHtmlAnchorElement;
            findAnotherLink.Text.Should().Be("Find a different employer");
            findAnotherLink.PathName.Should().Be($"/who-is-employer/{OpportunityId}-{OpportunityItemId}");

            var employerContact = documentHtml.QuerySelector("#EmployerContact") as IHtmlInputElement;
            employerContact.Value.Should().Be("Employer Contact");

            var employerContactEmail = documentHtml.QuerySelector("#EmployerContactEmail") as IHtmlInputElement;
            employerContactEmail.Value.Should().Be("employer-contact@email.com");

            var employerContactPhone = documentHtml.QuerySelector("#EmployerContactPhone") as IHtmlInputElement;
            employerContactPhone.Value.Should().Be("01474 787878");

            var confirmButton = documentHtml.QuerySelector("#tl-confirm") as IHtmlButtonElement;
            confirmButton.TextContent.Should().Be("Confirm and continue");
        }
    }
}