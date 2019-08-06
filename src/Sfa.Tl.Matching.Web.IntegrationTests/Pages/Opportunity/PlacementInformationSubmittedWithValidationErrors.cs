using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class PlacementInformationSubmittedWithValidationErrors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public PlacementInformationSubmittedWithValidationErrors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        // TODO FIX [InlineData("NoSuitableStudent", "", "You must tell us why the employer did not choose a provider", 0)]
        [InlineData("PlacementsKnown", "", "You must tell us whether the employer knows how many students they want for this job at this location", 2)]
        public async Task CorrectErrorMessageDisplayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"placement-information/{OpportunityItemId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.QuerySelector("button[id='tl-continue']"),
                new Dictionary<string, string>
                {
                    [field] = value
                });

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            
            response.EnsureSuccessStatusCode();

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");
            errorSummaryList.Children[errorSummaryIndex].TextContent.Should().Be(errorMessage);

            AssertError(responseContent, field, errorMessage);

            Assert.Null(response.Headers.Location?.OriginalString);
        }

        private static void AssertError(IHtmlDocument responseContent, string field, string errorMessage)
        {
            var placementsKnown = responseContent.QuerySelector($"#{field}");
            var placementsKnownDiv = placementsKnown.ParentElement.ParentElement;
            placementsKnownDiv.ClassName.Should().Be("govuk-form-group govuk-form-group--error");
            placementsKnownDiv.QuerySelector(".govuk-error-message").TextContent.Should()
                .Be(errorMessage);
        }
    }
}