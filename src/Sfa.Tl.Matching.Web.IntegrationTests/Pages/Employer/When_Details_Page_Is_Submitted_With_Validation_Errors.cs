using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Details_Page_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Details_Page_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("EmployerContact", "", "You must enter a contact name for placements", 0)]
        [InlineData("EmployerContact", "A", "You must enter a contact name using 2 or more characters", 0)]
        [InlineData("EmployerContact", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVW", "You must enter a contact name that is 100 characters or fewer", 0)]
        [InlineData("EmployerContact", "/%^*&$", "You must enter a contact name using only letters, hyphens and apostrophes", 0)]
        [InlineData("EmployerContact", "15151", "You must enter a contact name using only letters, hyphens and apostrophes", 0)]
        [InlineData("EmployerContactEmail", "", "You must enter a contact email for placements", 1)]
        [InlineData("EmployerContactEmail", "Abcd", "You must enter a valid email", 1)]
        [InlineData("EmployerContactPhone", "", "You must enter a contact telephone number for placements", 2)]
        [InlineData("EmployerContactPhone", "A", "You must enter a number", 2)]
        [InlineData("EmployerContactPhone", "123", "You must enter a telephone number that has 7 or more numbers", 2)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"employer-details/{OpportunityId}-{OpportunityItemId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-confirm"),
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

        private static void AssertError(IParentNode responseContent, string labelFor, string errorMessage)
        {
            var label = responseContent.QuerySelector($"label[for='{labelFor}']");
            var labelDiv = label.ParentElement;
            labelDiv.ClassName.Should().Be("govuk-form-group govuk-form-group--error");
            labelDiv.QuerySelector(".govuk-error-message").TextContent.Should()
                .Be(errorMessage);
        }
    }
}