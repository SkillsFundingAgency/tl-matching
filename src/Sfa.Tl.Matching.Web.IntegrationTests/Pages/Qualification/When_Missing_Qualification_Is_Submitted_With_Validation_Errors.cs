using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Qualification
{
    public class When_Missing_Qualification_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;
        private const int VenueId = 1;
        private const string LarsId = "12345678";

        public When_Missing_Qualification_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("ShortTitle", "ShortTitle", "", "You must enter a short title", 0)]
        [InlineData("shortTitleHidden", "ShortTitle", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVW", "You must enter a short title that is 100 characters or fewer", 0)]
        [InlineData("Routes_0__IsSelected", "Routes", "", "You must choose a skill area for this qualification", 1)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string fieldValMsg, string value, string errorMessage, int errorSummaryIndex)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"/missing-qualification/{VenueId}/{LarsId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-add"),
                new Dictionary<string, string>
                {
                    [field] = value
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            
            response.EnsureSuccessStatusCode();

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[errorSummaryIndex].TextContent.Should().Be(errorMessage);

            AssertError(responseContent, fieldValMsg, errorMessage);

            Assert.Null(response.Headers.Location?.OriginalString);
        }

        private static void AssertError(IParentNode responseContent, string fieldValMsg, string errorMessage)
        {
            var spanError = responseContent.QuerySelector($"span[data-valmsg-for='{fieldValMsg}']");
            spanError.ClassName.Should().Be("govuk-error-message field-validation-error");
            spanError.TextContent.Should().Be(errorMessage);
        }
    }
}