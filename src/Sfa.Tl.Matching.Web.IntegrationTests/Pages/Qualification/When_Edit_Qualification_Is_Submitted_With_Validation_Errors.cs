using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Qualification
{
    public class When_Edit_Qualification_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Edit_Qualification_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("SearchTerms", "", "You must enter 2 or more letters for your search", 0)]
        [InlineData("SearchTerms", "A", "You must enter 2 or more letters for your search", 0)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"/edit-qualifications");
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

            Assert.Null(response.Headers.Location?.OriginalString);
        }
    }
}