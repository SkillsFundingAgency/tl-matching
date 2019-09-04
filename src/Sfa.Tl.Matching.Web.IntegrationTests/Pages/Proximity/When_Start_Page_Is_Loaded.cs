using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Proximity
{
    public class When_Start_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Start_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Start");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Constants.ServiceName);

            var viewSavedOpportunitiesLink = documentHtml.GetElementById("tl-view-opportunities-link") as IHtmlAnchorElement;
            viewSavedOpportunitiesLink.PathName.Should().Be($"/saved-opportunities");

            var startLink = documentHtml.GetElementById("tl-start-now") as IHtmlAnchorElement;
            startLink.TextContent.Should().Be("Start a new opportunity");
            startLink.PathName.Should().Be($"/find-providers");

            var dataImportLink = documentHtml.GetElementById("tl-upload-link") as IHtmlAnchorElement;
            dataImportLink.TextContent.Should().Be("Upload employer and provider data");
            dataImportLink.PathName.Should().Be($"/DataImport");

            var editQualificationsLink = documentHtml.GetElementById("tl-edit-qualifications-link") as IHtmlAnchorElement;
            editQualificationsLink.TextContent.Should().Be("Edit qualifications");
            editQualificationsLink.PathName.Should().Be($"/edit-qualifications");

            var serviceMaintenanceLink = documentHtml.GetElementById("tl-maintenance-link") as IHtmlAnchorElement;
            serviceMaintenanceLink.TextContent.Should().Be("Service under maintenance");
            serviceMaintenanceLink.PathName.Should().Be($"/service-under-maintenance");
        }
    }
}