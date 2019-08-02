using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class EmployerConsentPageLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Confirm that we can share the employer’s contact details";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public EmployerConsentPageLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"permission/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be("Before you send emails");

            var name = documentHtml.QuerySelector("#tl-name") as IHtmlParagraphElement;
            name.TextContent.Should().Be("Employer Contact");

            var email = documentHtml.QuerySelector("#tl-email") as IHtmlParagraphElement;
            email.TextContent.Should().Be("employer-contact@email.com");

            var phone = documentHtml.QuerySelector("#tl-phone") as IHtmlParagraphElement;
            phone.TextContent.Should().Be("01474 787878");

            var changeLink = documentHtml.QuerySelector("#tl-change") as IHtmlAnchorElement;
            changeLink.Text.Should().Be("Change contact details");
            changeLink.PathName.Should().Be($"/check-employer-details/{OpportunityId}-{OpportunityItemId}");

            var confirmationLabel = documentHtml.QuerySelector("label[for='ConfirmationSelected']");
            confirmationLabel.TextContent.Should().Be("\n                                Employer Contact has confirmed that we can share their details with providers, and that these providers can contact them about industry placements.\n                            ");
        }
    }
}