using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    public class CreateSingleProvisionGap : IClassFixture<SeleniumWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly StartPage _startPage;
        private IWebDriver Driver { get; }

        public CreateSingleProvisionGap(SeleniumWebApplicationFactory<Startup> server)
        {
            server.CreateClient();
            var driverOptions = new ChromeOptions();
            driverOptions.AddArgument("--headless");
            Driver = new RemoteWebDriver(driverOptions);
            Driver.Navigate().GoToUrl(server.GetServerAddressForRelativeUrl("Start"));
            _startPage = new StartPage(Driver);
        }

        [Fact(DisplayName = "Create Single Referral")]
        public void CreateReferral()
        {
            _startPage.AssertContent();

            var proximityIndexPage = _startPage.ClickStart();
            proximityIndexPage.AssertContent();
            proximityIndexPage.EnterPostcode("CV1 2WT");

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