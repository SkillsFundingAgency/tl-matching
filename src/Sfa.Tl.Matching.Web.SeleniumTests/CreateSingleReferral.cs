using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    public class CreateSingleReferral : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly StartPage _startPage;
        public IWebDriver Driver { get; }

        public CreateSingleReferral(SeleniumServerFactory<Startup> server)
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

            var opportunityProximityIndexPage = _startPage.ClickStart();
            opportunityProximityIndexPage.AssertContent();
            opportunityProximityIndexPage.EnterPostcode("CV1 2WT");

            var opportunityProximityResultsPage = opportunityProximityIndexPage.ClickSearch();
            opportunityProximityResultsPage.AssertContent();
            opportunityProximityResultsPage.SelectProvider();

            var placementInformationPage = opportunityProximityResultsPage.ClickContinue();
            placementInformationPage.AssertContent();
            placementInformationPage.SelectPlacementsKnown();

            var findEmployerPage = placementInformationPage.ClickContinue();
            findEmployerPage.AssertContent();
            findEmployerPage = findEmployerPage.EnterCompanyName("Company Name For Selection");

            var detailsPage = findEmployerPage.ClickContinue();
            detailsPage.AssertContent();

            var checkAnswersPage = detailsPage.ClickConfirm();
            checkAnswersPage.AssertContent();

            var opportunityBasketPage = checkAnswersPage.ClickConfirm();
            opportunityBasketPage.AssertContent();

            //TODO: Add the following lines back - not working due BulkUpdate added by TLWP-962 
            //var employerConsentPage = opportunityBasketPage.ClickContinue();
            //employerConsentPage.AssertContent();
            //employerConsentPage = employerConsentPage.SelectConfirmationSelected();

            //var referralEmailSentPage = employerConsentPage.ClickConfirm();
            //referralEmailSentPage.AssertContent();
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}