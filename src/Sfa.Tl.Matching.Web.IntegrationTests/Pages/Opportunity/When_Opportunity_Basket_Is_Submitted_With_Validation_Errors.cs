using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Opportunity_Basket_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private const int OpportunityId = 1030;
        private const int OpportunityItemId = 1031;

        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        public When_Opportunity_Basket_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("ReferralItems_0__IsSelected", "ReferralItems[0].IsSelected", "", "You must select an opportunity to continue", 0)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string fieldValMsg, string value, string errorMessage, int errorSummaryIndex)
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"employer-opportunities/{OpportunityId}-{OpportunityItemId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-continue"),
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