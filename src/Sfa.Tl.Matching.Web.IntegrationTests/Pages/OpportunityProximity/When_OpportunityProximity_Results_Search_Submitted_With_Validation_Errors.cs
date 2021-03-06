﻿using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.OpportunityProximity
{
    public class When_OpportunityProximity_Results_Search_Submitted_With_Validation_Errors : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_OpportunityProximity_Results_Search_Submitted_With_Validation_Errors(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("Postcode", "", "You must enter a postcode", 0)]
        [InlineData("Postcode", "ABC", "You must enter a real postcode", 0)]
        public async Task Then_Correct_Error_Message_Is_Displayed(string field, string value, string errorMessage, int errorSummaryIndex)
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-SW1A%202AA-for-route-1");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.GetElementById("tl-search-form"),
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

            AssertError(responseContent, field);

            Assert.Null(response.Headers.Location?.OriginalString);
        }

        private static void AssertError(IParentNode responseContent, string field)
        {
            var input = responseContent.QuerySelector($"#{field}");
            var div = input.ParentElement;
            div.ClassName.Should().Be("govuk-form-group govuk-form-group--error");

            //No error message shown on this page, just the error border
            var errorSpan = div.QuerySelector(".govuk-error-message") as IHtmlSpanElement;
            errorSpan.Should().BeNull();
        }
    }
}