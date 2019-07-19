using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class CreateReferral : IClassFixture<DotNetChromeFixture>
    {
        private readonly IWebDriver _driver;
        private readonly StartPage _startPage;

        public CreateReferral(DotNetChromeFixture dotNetChromeFixture)
        {
            _driver = dotNetChromeFixture.WebDriver;

            var idamsLogin = new IdamsLogin(_driver);
            _startPage = idamsLogin.ClickSignInAsAdmin();
        }

        [Fact(DisplayName = "Create Referral")]
        public void CreateReferral1()
        {
            _startPage.AssertContent();
        }
    }
}