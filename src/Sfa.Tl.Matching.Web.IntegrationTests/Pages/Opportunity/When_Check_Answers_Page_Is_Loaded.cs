using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Check_Answers_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "Check answers";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private const int OpportunityProviderMultipleId = 1060;
        private const int OpportunityItemProviderMultipleId = 1061;
        private const int ProviderReferral1Id = 1062;
        private const int ProviderReferral2Id = 1063;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Check_Answers_Page_Is_Loaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"check-answers/{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            //var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            //backLink.Text.Should().Be("Back");
            //backLink.PathName.Should().Be($"/get-placement-or-employer/{OpportunityId}-{OpportunityItemId}");

            var cancelLink = documentHtml.GetElementById("tl-finish") as IHtmlAnchorElement;
            cancelLink.PathName.Should().Be($"/remove-opportunityItem/{OpportunityId}-{OpportunityItemId}");

            var providerResultsUrl =
                $"/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-SW1A%202AA-for-route-1";

            var placementInformationTable = documentHtml.GetElementById("tl-placement-table") as IHtmlElement;

            var placementKey = placementInformationTable.GetElementsByClassName("govuk-summary-list__key");
            
            placementKey[0].InnerHtml.Trim().Should().Be("Skill area");
            placementKey[1].InnerHtml.Trim().Should().Be("Postcode of workplace");
            placementKey[2].InnerHtml.Trim().Should().Be("Job role");
            placementKey[3].InnerHtml.Trim().Should().Be("Students wanted");

            var placementValue = placementInformationTable.GetElementsByClassName("govuk-summary-list__value");

            placementValue[0].InnerHtml.Trim().Should().Be("Agriculture, environmental and animal care");
            placementValue[1].InnerHtml.Trim().Should().Be("SW1A 2AA");
            placementValue[2].InnerHtml.Trim().Should().Be("Job Role");
            placementValue[3].InnerHtml.Trim().Should().Be("1");

            var placementActions = placementInformationTable.GetElementsByClassName("govuk-link") ;

            placementActions[0].TextContent.Should().Be("Change the type of placement");
            placementActions[1].TextContent.Should().Be("Change the postcode of the workplace");
            placementActions[2].TextContent.Should().Be("Change the job role");
            placementActions[3].TextContent.Should().Be("Change the number of placements");
            
            var changeProvidersLink = documentHtml.GetElementById("tl-change-providers") as IHtmlAnchorElement;
            changeProvidersLink.TextContent.Should().Be("Change providers");
            changeProvidersLink.PathName.Should().Be(providerResultsUrl);

            var confirmButton = documentHtml.GetElementById("tl-confirm") as IHtmlButtonElement;
            confirmButton.TextContent.Should().Be("Confirm and save opportunity");
            confirmButton.Type.Should().Be("submit");
        }

        [Fact]
        public async Task Then_Correct_Response_With_RemoveLink_For_Providers_Is_Returned()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync($"check-answers/{OpportunityItemProviderMultipleId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var providerResultsUrl =
                $"/provider-results-for-opportunity-{OpportunityProviderMultipleId}-item-{OpportunityItemProviderMultipleId}-within-30-miles-of-SW1A%202AA-for-route-1";

            var placementInformationTable = documentHtml.GetElementById("tl-placement-table") as IHtmlElement;

            var placementKey = placementInformationTable.GetElementsByClassName("govuk-summary-list__key");
            var placementValue = placementInformationTable.GetElementsByClassName("govuk-summary-list__value");
            var placementActions = placementInformationTable.GetElementsByClassName("govuk-link");

            placementKey[0].InnerHtml.Trim().Should().Be("Skill area");
            placementKey[1].InnerHtml.Trim().Should().Be("Postcode of workplace");
            placementKey[2].InnerHtml.Trim().Should().Be("Job role");
            placementKey[3].InnerHtml.Trim().Should().Be("Students wanted");
            
            placementValue[0].InnerHtml.Trim().Should().Be("Agriculture, environmental and animal care");
            placementValue[1].InnerHtml.Trim().Should().Be("SW1A 2AA");
            placementValue[2].InnerHtml.Trim().Should().Be("Job Role");
            placementValue[3].InnerHtml.Trim().Should().Be("1");

            placementActions[0].TextContent.Should().Be("Change the type of placement");
            placementActions[1].TextContent.Should().Be("Change the postcode of the workplace");
            placementActions[2].TextContent.Should().Be("Change the job role");
            placementActions[3].TextContent.Should().Be("Change the number of placements");

            // Assert Provider Information with Remove Link

            var providerTable = documentHtml.GetElementById("tl-providers-table");

            var providerKey = providerTable.GetElementsByClassName("govuk-summary-list__key");
            var providerValue = providerTable.GetElementsByClassName("govuk-summary-list__value");
            var providerActions = providerTable.GetElementsByClassName("govuk-link");

            providerKey[0].InnerHtml.Trim().Should().Be("Venue 1 Name (CV1 2WT)");
            providerKey[1].InnerHtml.Trim().Should().Be("Venue 2 Name (CV1 1EE)");

            providerValue[0].InnerHtml.Trim().Should().Be("1.2 miles from SW1A 2AA");
            providerValue[1].InnerHtml.Trim().Should().Be("2.9 miles from SW1A 2AA");

            providerActions.Select(x => x.TextContent).Should().BeEquivalentTo(new[] {"Remove", "Remove"});
            
            var changeProvidersLink = documentHtml.QuerySelector("#tl-change-providers") as IHtmlAnchorElement;
            changeProvidersLink.TextContent.Should().Be("Change providers");
            changeProvidersLink.PathName.Should().Be(providerResultsUrl);
        }
    }
}