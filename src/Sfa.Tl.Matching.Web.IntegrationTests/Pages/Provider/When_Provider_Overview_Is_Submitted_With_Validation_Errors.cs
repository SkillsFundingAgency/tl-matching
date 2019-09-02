using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Provider
{
    public class When_Provider_Overview_Is_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private readonly CustomWebApplicationFactory<TestStartup> _factory;
        private const int ProviderId = 1;

        public When_Provider_Overview_Is_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("DisplayName", "", "You must tell us how the provider name should be displayed", 0)]
        [InlineData("DisplayName", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJK", "You must enter a provider name that is 400 characters or fewer", 0)]
        [InlineData("PrimaryContact", "", "You must tell us who the primary contact is for industry placements", 2)]
        [InlineData("PrimaryContact", "A", "You must enter a contact name using 2 or more characters", 2)]
        [InlineData("PrimaryContact", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVW", "You must enter a contact name that is 100 characters or fewer", 2)]
        [InlineData("PrimaryContact", "123", "You must enter a contact name using letters", 2)]
        [InlineData("PrimaryContactEmail", "", "You must enter an email for the primary contact", 3)]
        [InlineData("PrimaryContactEmail", "Abcd", "Enter an email address in the correct format, like name@example.com", 3)]
        [InlineData("PrimaryContactPhone", "", "You must enter a telephone number for the primary contact number", 4)]
        [InlineData("PrimaryContactPhone", "A", "You must enter a telephone number using numbers", 4)]
        [InlineData("PrimaryContactPhone", "123", "You must enter a telephone number that has 7 or more numbers", 4)]
        [InlineData("SecondaryContact", "A", "You must enter a contact name using 2 or more characters", 5)]
        [InlineData("SecondaryContact", "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVW", "You must enter a contact name that is 100 characters or fewer", 5)]
        [InlineData("SecondaryContact", "123", "You must enter a contact name using letters", 5)]
        [InlineData("SecondaryContactEmail", "Abcd", "Enter an email address in the correct format, like name@example.com", 6)]
        [InlineData("SecondaryContactPhone", "A", "You must enter a telephone number using numbers", 7)]
        [InlineData("SecondaryContactPhone", "123", "You must enter a telephone number that has 7 or more numbers", 7)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"/provider-overview/{ProviderId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-save-finish"),
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