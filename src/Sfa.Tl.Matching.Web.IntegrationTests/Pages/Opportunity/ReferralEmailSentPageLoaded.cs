using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class ReferralEmailSentPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Emails sent";
        private const int OpportunityId = 1000;

        private readonly CustomWebApplicationFactory<Startup> _factory;

        public ReferralEmailSentPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"emails-sent/{OpportunityId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-panel__title");
            header1.TextContent.Should().Be(Title);

            var crmLink = documentHtml.QuerySelector("#tl-crm-link") as IHtmlAnchorElement;
            // TODO FIX crmLink.Href.Should().Be("https://esfa-cs-prod.crm4.dynamics.com/main.aspx?pagetype=entityrecord&amp;etc=1&amp;id=%7b65351B3C-FAF8-4752-8806-8D6E240C334E%7d&amp;extraqs=&amp;newWindow=true");

            var endButton = documentHtml.QuerySelector("#tl-end") as IHtmlAnchorElement;
            endButton.TextContent.Should().Be("Finish");

            var feedbackLink = documentHtml.QuerySelector("#tl-feedback-survey") as IHtmlAnchorElement;
            feedbackLink.TextContent.Should().Be("Give us feedback");
            feedbackLink.PathName.Should().Be("/feedback");
        }
    }
}