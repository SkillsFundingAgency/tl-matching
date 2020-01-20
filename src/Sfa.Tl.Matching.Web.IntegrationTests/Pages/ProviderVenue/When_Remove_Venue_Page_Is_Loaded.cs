using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.ProviderVenue
{
    public class When_Remove_Venue_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private const int VenueId = 1;

        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        public When_Remove_Venue_Page_Is_Loaded(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/remove-venue/{VenueId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var postcode = "CV1 2WT";

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);
            documentHtml.Title.Should().Be($"Remove {postcode}? - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be($"Are you sure you want to remove {postcode}?");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/get-admin-back-link/0");

            var removeButton = documentHtml.GetElementById("tl-remove") as IHtmlButtonElement;
            removeButton.TextContent.Should().Be("Remove venue");
            removeButton.Type.Should().Be("submit");

            var cancelLink = documentHtml.GetElementById("tl-cancel") as IHtmlAnchorElement;
            cancelLink.Text.Should().Be("Cancel and return to venue overview");
            cancelLink.PathName.Should().Be($"/venue-overview/{VenueId}");
        }
    }
}