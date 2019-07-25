using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.IntegrationTests.Specflow.Helpers;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Proximity
{
    public class ProximityIndexPageLoaded : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private const string Title = "Set up placement opportunity";

        private readonly HttpResponseMessage _response;

        public ProximityIndexPageLoaded(CustomWebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateClient();

            _response = client.GetAsync("/find-providers").GetAwaiter().GetResult();
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

            var routeList = indexViewHtml.GetElementById("SelectedRouteId");
            routeList.Children.Length.Should().Be(2);
            routeList.Text().Should().Be("Route 1\nRoute 2\n");

            var postcode = indexViewHtml.GetElementById("Postcode");
            postcode.Should().NotBeNull();

            var search = indexViewHtml.GetElementById("tl-search");
            search.Should().NotBeNull();
        }
    }
}