using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Home
{
    public class When_Maintenance_Page_Is_Loaded_With_Service_Online : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Service under maintenance";

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Maintenance_Page_Is_Loaded_With_Service_Online(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task The_The_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/service-under-maintenance");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/get-admin-back-link/0");

            var body = documentHtml.QuerySelector(".govuk-body");
            body.TextContent.Should().Be("The service is currently online.");

            var maintenanceButton = documentHtml.GetElementById("tl-maintenance") as IHtmlButtonElement;
            maintenanceButton.TextContent.Trim().Should().Be("Take offline");
            maintenanceButton.Type.Should().Be("submit");
        }
    }
}