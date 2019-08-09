using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class ProvidersPageLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Select providers for this opportunity";
        private const int OpportunityId = 0;
        private const int OpportunityItemId = 0;
        private const int SearchRadius = 25;
        private const string Postcode = "CV1 2WT";
        private const int SelectedRouteId = 1;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public ProvidersPageLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var resultText = documentHtml.QuerySelector(".govuk-body");
            resultText.TextContent.Trim().Should().Be("2 results in Agriculture, environmental and animal care within 25 miles of CV1 2WT");
         
            //TODO: Add more test cases
            
        }


    }
}
