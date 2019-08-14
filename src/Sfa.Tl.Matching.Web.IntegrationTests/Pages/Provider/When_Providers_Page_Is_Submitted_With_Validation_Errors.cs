using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Providers_Page_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        private const string Title = "Select providers for this opportunity";
        private const int OpportunityId = 0;
        private const int OpportunityItemId = 0;
        private const int SearchRadius = 25;
        private const string Postcode = "CV1 2WT";
        private const int SelectedRouteId = 1;

        public When_Providers_Page_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("You must select at least one provider", 0)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string errorMessage, int errorSummaryIndex)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}");

            pageResponse.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                pageResponse.Content.Headers.ContentType.ToString());

            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            pageContent.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = pageContent.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.GetElementById("tl-result-form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-continue"));

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);

            response.EnsureSuccessStatusCode();

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);
            var errorSummaryList = responseContent.QuerySelector(".govuk-error-summary div ul");

            errorSummaryList.Children[errorSummaryIndex].TextContent.Should().Be(errorMessage);
        }
    }
}
