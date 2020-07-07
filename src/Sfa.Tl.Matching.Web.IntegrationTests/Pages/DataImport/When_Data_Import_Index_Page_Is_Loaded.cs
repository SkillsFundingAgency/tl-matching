using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.DataImport
{
    public class When_Data_Import_Index_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Data Import";

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Data_Import_Index_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_The_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/DataImport");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            backLink.PathName.Should().Be("/Start");
            
            var importTypeList = documentHtml.GetElementById("SelectedImportType");
            importTypeList.Children.Length.Should().Be(4);
            importTypeList.Children[0].Text().Should().Be("Learning Aim Reference");
            importTypeList.Children[1].Text().Should().Be("Local Enterprise Partnership");
            importTypeList.Children[2].Text().Should().Be("ONS Postcodes");
            importTypeList.Children[3].Text().Should().Be("CDF Provider Update");

            var upload = documentHtml.GetElementById("tl-upload") as IHtmlButtonElement;
            upload.TextContent.Trim().Should().Be("Upload");
            upload.Type.Should().Be("submit");
        }
    }
}