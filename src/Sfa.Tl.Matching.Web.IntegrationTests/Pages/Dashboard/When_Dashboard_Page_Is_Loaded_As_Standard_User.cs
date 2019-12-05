using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Dashboard
{
    public class When_Dashboard_Page_Is_Loaded_As_Standard_User : IClassFixture<CustomWebApplicationFactory<StandardUserTestStartup>>
    {
        private readonly CustomWebApplicationFactory<StandardUserTestStartup> _factory;

        public When_Dashboard_Page_Is_Loaded_As_Standard_User(CustomWebApplicationFactory<StandardUserTestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_The_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/start");

            response.EnsureSuccessStatusCode();

            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Constants.ServiceName);

            var startLink = documentHtml.GetElementById("tl-dash-createnew") as IHtmlAnchorElement;
            startLink.TextContent.Trim().Should().Be("Create and refer new opportunities\n                    Find relevant, local providers to introduce to your employer.");
            startLink.PathName.Should().Be("/find-providers");

            var searchPostcodeLink = documentHtml.GetElementById("tl-dash-showall") as IHtmlAnchorElement;
            searchPostcodeLink.TextContent.Trim().Should().Be("Show all providers in an area\n                    Search a geographical area for everything that’s available.");
            searchPostcodeLink.PathName.Should().Be("/find-all-providers");

            var viewSavedOpportunitiesLink = documentHtml.GetElementById("tl-dash-viewsaved") as IHtmlAnchorElement;
            viewSavedOpportunitiesLink.TextContent.Trim().Should().Be("View saved opportunities\n                        Any unreferred opportunities that you’re working on.");
            viewSavedOpportunitiesLink.PathName.Should().Be("/saved-opportunities");

            //Admin Links

            var editProviderLink = documentHtml.GetElementById("tl-dash-manageprovider") as IHtmlAnchorElement;
            editProviderLink.Should().BeNull();

            var editQualificationsLink = documentHtml.GetElementById("tl-dash-editquals") as IHtmlAnchorElement;
            editQualificationsLink.Should().BeNull();

            var dataImportLink = documentHtml.GetElementById("tl-dash-uploaddata") as IHtmlAnchorElement;
            dataImportLink.Should().BeNull();

            var serviceMaintenanceLink = documentHtml.GetElementById("tl-dash-takeoffline") as IHtmlAnchorElement;
            serviceMaintenanceLink.Should().BeNull();
        }

    }
}
