using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Provider_Search_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Provider_Search_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("UkPrn", "", "You must enter a UKPRN", 0)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync("search-ukprn");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-search"),
                new Dictionary<string, string>
                {
                    [field] = value
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            
            response.EnsureSuccessStatusCode();

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[errorSummaryIndex].TextContent.Should().Be(errorMessage);

            var errorSpan = responseContent.QuerySelector(".govuk-error-message");
            errorSpan.TextContent.Should().Be(errorMessage);
            errorSpan.ClassName.Should().Be("govuk-error-message field-validation-error");

            var showAllProvidersLink = responseContent.GetElementById("tl-show-all") as IHtmlAnchorElement;
            showAllProvidersLink.Text.Should().Be("Show all providers");
            showAllProvidersLink.PathName.Should().Be("/search-ukprn");

            Assert.Null(response.Headers.Location?.OriginalString);
        }
    }
}