using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    public class CreateSingleProvisionGap : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly StartPage _startPage;
        public IWebDriver Driver { get; }

        public CreateSingleProvisionGap(SeleniumServerFactory<Startup> server)
        {
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArgument("--headless");
            var driver = new RemoteWebDriver(opts);
            driver.Navigate().GoToUrl(server.RootUri + "/Start");
            _startPage = new StartPage(driver);

            Driver = driver;
        }

        [Fact(DisplayName = "Create Single Referral")]
        public void CreateReferral()
        {
            _startPage.AssertContent();

            var proximityIndexPage = _startPage.ClickStart();
            proximityIndexPage.AssertContent();
            proximityIndexPage.EnterPostcode("SW1A 2AA");

            var proximityResultsPage = proximityIndexPage.ClickSearch();
            proximityResultsPage.AssertContent();

            var placementInformationPage = proximityResultsPage.ClickNoSuitableProvidersLink();
            placementInformationPage.AssertContent();
            placementInformationPage.SelectReasonNoProvider();
            placementInformationPage.SelectPlacementsKnown();

            var findEmployerPage = placementInformationPage.ClickContinue();
            findEmployerPage.AssertContent();
            findEmployerPage = findEmployerPage.EnterCompanyName("Company Name For Selection");

            var detailsPage = findEmployerPage.ClickContinue();
            detailsPage.AssertContent();

            var opportunityBasketPage = detailsPage.ClickConfirmToOpprunityBasket();
            opportunityBasketPage.AssertContent();
            opportunityBasketPage.ClickFinish();
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}