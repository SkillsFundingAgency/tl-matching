using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Employer_Consent_Page_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Employer_Consent_Page_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CorrectErrorMessageDisplayed()
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"permission/{OpportunityId}-{OpportunityItemId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.QuerySelector("button[id='tl-continue']"),
                new Dictionary<string, string>
                {
                    ["ConfirmationSelected"] = "false"
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            
            response.EnsureSuccessStatusCode();

            const string errorMessage =
                "You must confirm that we can share the employer’s details with the selected providers";

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[0].TextContent.Should().Be(errorMessage);

            var confirmationSelectedErrorSpan = responseContent.QuerySelector("span[data-valmsg-for='ConfirmationSelected']");
            confirmationSelectedErrorSpan.TextContent.Should().Be(errorMessage);

            Assert.Null(response.Headers.Location?.OriginalString);
        }
    }
}