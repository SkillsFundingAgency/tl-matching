using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class When_Opportunity_Basket_Contains_Multiple_Items_And_Submitted_With_Multiple_Referrals : IClassFixture<CustomWebApplicationFactory<SqlServerStartup>>
    {
        private const int OpportunityId = 3000;
        private readonly CustomWebApplicationFactory<SqlServerStartup> _factory;

        public When_Opportunity_Basket_Contains_Multiple_Items_And_Submitted_With_Multiple_Referrals(CustomWebApplicationFactory<SqlServerStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Then_Submit_Form_Successfully()
        {
            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(200);
           
            var pageResponse = await client.GetAsync($"employer-opportunities/{OpportunityId}-0");
            var pageContent = await HtmlHelpers.GetDocumentAsync(pageResponse);

            var response = await client.SendAsync(
                 (IHtmlFormElement)pageContent.QuerySelector("form"),
                 (IHtmlButtonElement)pageContent.GetElementById("tl-continue"),
                 new Dictionary<string, string>
                 {
                     ["OpportunityId"] = "3000",
                     ["OpportunityItemId"] = "0",
                     ["SelectedOpportunity[100].IsSelected"] = "true",
                     ["SelectedOpportunity[200].IsSelected"] = "true",
                     ["SelectedOpportunity[300].IsSelected"] = "true",
                     ["SelectedOpportunity[400].IsSelected"] = "true",
                     ["SelectedOpportunity[500].IsSelected"] = "true"
                 });

            var responseContent = await HtmlHelpers.GetDocumentAsync(response);

            Assert.Equal(HttpStatusCode.OK, pageResponse.StatusCode);
            Assert.Equal("permission/3000-0", responseContent.BaseUrl.Path);
        }

    }

}
