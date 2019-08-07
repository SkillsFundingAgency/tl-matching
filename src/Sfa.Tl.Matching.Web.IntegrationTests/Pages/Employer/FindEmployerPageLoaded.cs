using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class FindEmployerPageLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        private const string Title = "Who is the employer?";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        public FindEmployerPageLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"who-is-employer/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var companyName = documentHtml.QuerySelector("#companyNameHidden") as IHtmlInputElement;
            companyName.Value.Should().Be("Company Name");

            var continueButton = documentHtml.QuerySelector("#tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue");
        }
    }
}