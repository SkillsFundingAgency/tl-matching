using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Sfa.Tl.Matching.Web.IntegrationTests.Selenium.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Selenium
{
    public class CreateSingleReferral : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly SeleniumServerFactory<Startup> _server;

        private readonly StartPage _startPage;
        public IWebDriver Driver { get; }

        public CreateSingleReferral(SeleniumServerFactory<Startup> server)
        {
            _server = server;
            _server.CreateClient();
            var opts = new ChromeOptions();
            //opts.AddArgument("--headless");
            //opts.SetLoggingPreference(OpenQA.Selenium.LogType.Browser, LogLevel.All);
            var driver = new RemoteWebDriver(opts);
            driver.Navigate().GoToUrl(_server.RootUri + "/Start");
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
            proximityResultsPage.SelectProvider();
            proximityResultsPage.EnterSearchRadius("25 miles");

            var placementInformationPage = proximityResultsPage.ClickContinue();
            placementInformationPage.AssertContent();
            placementInformationPage = placementInformationPage.SelectPlacementsKnown();

            var findEmployerPage = placementInformationPage.ClickContinue();
            findEmployerPage.AssertContent();
            findEmployerPage = findEmployerPage.EnterCompanyName("Company Name");

            var detailsPage = findEmployerPage.ClickContinue();
            detailsPage.AssertContent();

            var checkAnswersPage = detailsPage.ClickConfirm();
            checkAnswersPage.AssertContent();
            //checkAnswersPage.AssertDatabase();

            var opportunityBasketPage = checkAnswersPage.ClickConfirm();
            opportunityBasketPage.AssertContent();
            //opportunityBasketPage.AssertDatabase();

            var employerConsentPage = opportunityBasketPage.ClickContinue();
            employerConsentPage.AssertContent();
            employerConsentPage = employerConsentPage.SelectConfirmationSelected();

            var referralEmailSentPage = employerConsentPage.ClickConfirm();
            referralEmailSentPage.AssertContent();
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}