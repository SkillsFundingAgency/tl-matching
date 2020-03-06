using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.DataImport
{
    public class When_Data_Import_Index_Page_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Data_Import_Index_Page_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("", "", null, DataImportType.LearningAimReference, "You must select a file", 0)]
        public async Task Then_The_Correct_Error_Message_Is_Displayed(string fileName, string fileContentType, string fileContent, DataImportType importType, string errorMessage, int errorSummaryIndex)
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync("/DataImport");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var file = Substitute.For<IFile>();
            file.Name.Returns(fileName);
            file.Type.Returns(fileContentType);
            file.Body.Returns(fileContent != null
                ? new MemoryStream(Encoding.UTF8.GetBytes(fileContent))
                : null);

            var input = (IHtmlInputElement)pageContent.QuerySelector("input[type=file][name=File]");
            input?.Files.Add(file);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-upload"),
                new Dictionary<string, string>
                {
                    ["SelectedImportType"] = importType.ToString()
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);

            response.EnsureSuccessStatusCode();

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[errorSummaryIndex].TextContent.Should().Be(errorMessage);

            AssertError(responseContent, "File", errorMessage);

            Assert.Null(response.Headers.Location?.OriginalString);
        }

        private static void AssertError(IParentNode responseContent, string field, string errorMessage)
        {
            var input = responseContent.QuerySelector($"#{field}");
            var div = input.ParentElement;
            div.ClassName.Should().Be("govuk-form-group govuk-form-group--error");
            div.QuerySelector(".govuk-error-message").TextContent.Should()
                .Be(errorMessage);
        }
    }
}