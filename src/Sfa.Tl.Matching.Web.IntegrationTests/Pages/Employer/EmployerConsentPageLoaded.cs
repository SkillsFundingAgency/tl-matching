using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.IntegrationTests.Specflow.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class EmployerConsentPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Confirm that we can share the employer’s contact details";

        private readonly HttpResponseMessage _response;
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;

        public EmployerConsentPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            _response = client.GetAsync($"check-employer-details/{OpportunityId}-{OpportunityItemId}").GetAwaiter().GetResult();
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