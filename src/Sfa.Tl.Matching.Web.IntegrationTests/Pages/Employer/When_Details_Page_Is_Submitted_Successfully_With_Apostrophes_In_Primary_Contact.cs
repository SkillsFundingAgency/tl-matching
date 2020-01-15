using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Employer
{
    public class When_Details_Page_Is_Submitted_Successfully_With_Apostrophes_In_Primary_Contact : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const int OpportunityId = 1000;
        private const int OpportunityItemId = 2000;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public When_Details_Page_Is_Submitted_Successfully_With_Apostrophes_In_Primary_Contact(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Redirect_Is_Correct()
        {
            var client = _factory.CreateClient();
            var pageResponse = await client.GetAsync($"employer-details/{OpportunityId}-{OpportunityItemId}");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                (IHtmlFormElement)pageContent.QuerySelector("form"),
                (IHtmlButtonElement)pageContent.GetElementById("tl-confirm"),
                new Dictionary<string, string>
                {
                    //Test with two kinds of apostrophes
                    ["PrimaryContact"] = "O’Neil O'Malley",
                    ["Email"] = "email@address.com",
                    ["Phone"] = "0123456789"
                });

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            Assert.Equal("check-answers/2000", responseContent.BaseUrl.Path);
        }
    }
}