using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Find_Employer_Page_Is_Loaded : IClassFixture<CustomWebApplicationFactory<InMemoryStartup>>
    {
        private readonly CustomWebApplicationFactory<InMemoryStartup> _factory;

        private const string Title = "Who is the employer?";
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        public When_Find_Employer_Page_Is_Loaded(CustomWebApplicationFactory<InMemoryStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Correct_Response_Is_Returned()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"who-is-employer/{OpportunityId}-{OpportunityItemId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var companyName = documentHtml.GetElementById("companyNameHidden") as IHtmlInputElement;
            companyName.Value.Should().Be("Company Name");

            //var backLink = documentHtml.GetElementById("tl-back") as IHtmlAnchorElement;
            //backLink.Text.Should().Be("Back");
            //backLink.PathName.Should().Be($"/placement-information/{OpportunityItemId}");

            var continueButton = documentHtml.GetElementById("tl-continue") as IHtmlButtonElement;
            continueButton.TextContent.Should().Be("Continue");
            continueButton.Type.Should().Be("submit");
        }
    }
}