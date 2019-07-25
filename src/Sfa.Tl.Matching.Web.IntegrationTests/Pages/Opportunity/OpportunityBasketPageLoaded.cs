using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.IntegrationTests.Specflow.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class OpportunityBasketPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;

        private readonly HttpResponseMessage _response;

        public OpportunityBasketPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            _response = client.GetAsync($"employer-opportunities/{OpportunityId}-{OpportunityItemId}").GetAwaiter().GetResult();
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            _response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                _response.Content.Headers.ContentType.ToString());

            var indexViewHtml = await HtmlHelpers.GetDocumentAsync(_response);

            indexViewHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = indexViewHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);









        }
    }
}