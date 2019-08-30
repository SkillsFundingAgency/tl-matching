using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Employer_Consent_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Confirm that we can share the employer’s contact details";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Employer_Consent_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"permission/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be("Before you send emails");

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            //var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            //backLink.Text.Should().Be("Back");
            //backLink.PathName.Should().Be($"/employer-opportunities/{OpportunityId}-{OpportunityItemId}");

            var saveLink = documentHtml.GetElementById("tl-save") as IHtmlAnchorElement;
            saveLink.Text.Should().Be("Save and come back later");
            saveLink.PathName.Should().Be($"/save-employer-opportunity/{OpportunityId}");

            var name = documentHtml.GetElementById("tl-name") as IHtmlParagraphElement;
            name.TextContent.Should().Be("Employer Contact");

            var email = documentHtml.GetElementById("tl-email") as IHtmlParagraphElement;
            email.TextContent.Should().Be("employer-contact@email.com");

            var phone = documentHtml.GetElementById("tl-phone") as IHtmlParagraphElement;
            phone.TextContent.Should().Be("01474 787878");

            var changeLink = documentHtml.GetElementById("tl-change") as IHtmlAnchorElement;
            changeLink.Text.Should().Be("Change contact details");
            changeLink.PathName.Should().Be($"/check-employer-details/{OpportunityId}-{OpportunityItemId}");

            var confirmationLabel = documentHtml.QuerySelector("label[for='ConfirmationSelected']");
            confirmationLabel.TextContent.Should().Be("\n                                Employer Contact has confirmed that we can share their details with providers, and that these providers can contact them about industry placements.\n                            ");
        }
    }
}