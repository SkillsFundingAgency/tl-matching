using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Qualification
{
    public class When_Add_Qualification_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Add qualification";
        private const int VenueId = 1;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Add_Qualification_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/add-qualification/{VenueId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be("Add a qualification for CV1 2WT");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/get-admin-back-link");

            var larId = documentHtml.GetElementById("LarId") as IHtmlInputElement;
            larId.Value.Should().BeEmpty();

            var venueId = documentHtml.GetElementById("ProviderVenueId") as IHtmlInputElement;
            venueId.Type.Should().Be("hidden");
            venueId.Value.Should().Be("1");

            var postcode = documentHtml.GetElementById("Postcode") as IHtmlInputElement;
            postcode.Type.Should().Be("hidden");
            postcode.Value.Should().Be("CV1 2WT");

            var source = documentHtml.GetElementById("Source") as IHtmlInputElement;
            source.Type.Should().Be("hidden");
            source.Value.Should().Be("Admin");

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue");
            continueButton.Type.Should().Be("submit");
        }
    }
}