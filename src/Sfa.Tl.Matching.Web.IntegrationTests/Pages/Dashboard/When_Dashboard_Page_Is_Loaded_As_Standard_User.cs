using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Dashboard
{
    public class When_Dashboard_Page_Is_Loaded_As_Standard_User : IClassFixture<CustomWebApplicationFactory<StandardUserTestStartup>>
    {
        private const string Title = "Match employers with providers for industry placements";
        private readonly CustomWebApplicationFactory<StandardUserTestStartup> _factory;

        public When_Dashboard_Page_Is_Loaded_As_Standard_User(CustomWebApplicationFactory<StandardUserTestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_The_Correct_Response_Is_Returned()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/start");

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Constants.ServiceName} - GOV.UK");
        }

    }
}
