using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.OpportunityProximity
{
    public class When_OpportunityProximity_Results_Continue_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_OpportunityProximity_Results_Continue_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Error_Message_Is_Displayed()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-CV1%202WT-for-route-1");
                
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.GetElementById("tl-result-form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-continue"),
                new Dictionary<string, string>
                {
                    ["SelectedProvider[0].IsSelected"] = "false"
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);

            response.EnsureSuccessStatusCode();

            const string errorMessage = "You must select at least one provider";

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[0].TextContent.Should().Be(errorMessage);

            var input = responseContent.QuerySelector($".tl-search-results");
            var div = input.ParentElement;
            div.ClassName.Should().Be("govuk-form-group govuk-form-group--error");

            var errorSpan = div.QuerySelector(".govuk-error-message") as IHtmlSpanElement;
            errorSpan.TextContent.Should().Be(errorMessage);

            Assert.Null(response.Headers.Location?.OriginalString);
        }
    }
}