﻿using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Details_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Confirm contact details for industry placements";
        private const string HeaderText = "Confirm that the employer’s contact details for industry placements are accurate and up-to-date";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Details_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            
            var response = await client.GetAsync($"employer-details/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(HeaderText);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            backLink.Text.Should().Be("Back");
            //backLink.PathName.Should().Be($"/who-is-employer/{OpportunityId}/{OpportunityItemId}");
            backLink.PathName.Should().Be($"/get-back-link/{OpportunityId}/{OpportunityItemId}/0/0");

            var cancelLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            cancelLink.PathName.Should().Be($"/remove-opportunityItem/{OpportunityId}-{OpportunityItemId}");

            var findAnotherLink = documentHtml.GetElementById("tl-find-different") as IHtmlAnchorElement;
            findAnotherLink.Text.Should().Be("Find a different employer");
            findAnotherLink.PathName.Should().Be($"/who-is-employer/{OpportunityId}-{OpportunityItemId}");

            var employerContact = documentHtml.GetElementById("PrimaryContact") as IHtmlInputElement;
            employerContact.Value.Should().Be("Employer Contact");

            var employerContactEmail = documentHtml.GetElementById("Email") as IHtmlInputElement;
            employerContactEmail.Value.Should().Be("employer-contact@email.com");

            var employerContactPhone = documentHtml.GetElementById("Phone") as IHtmlInputElement;
            employerContactPhone.Value.Should().Be("01474 787878");

            var confirmButton = documentHtml.GetElementById("tl-confirm") as IHtmlButtonElement;
            confirmButton.TextContent.Should().Be("Confirm and continue");
            confirmButton.Type.Should().Be("submit");
        }
    }
}