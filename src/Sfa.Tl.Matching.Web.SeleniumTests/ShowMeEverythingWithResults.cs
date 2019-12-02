using System;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.SeleniumTests
{
    public class ShowMeEverythingWithResults : IClassFixture<SeleniumServerFactory<Startup>>, IDisposable
    {
        private readonly StartPage _startPage;
        public IWebDriver Driver { get; }

        public ShowMeEverythingWithResults(SeleniumServerFactory<Startup> server)
        {
            server.CreateClient();
            var opts = new ChromeOptions();
            opts.AddArgument("--headless");
            var driver = new RemoteWebDriver(opts);
            driver.Navigate().GoToUrl(server.RootUri + "/Start");
            _startPage = new StartPage(driver);

            Driver = driver;
        }

        [Fact(DisplayName = "Search")]
        public void Search()
        {
            _startPage.AssertContent();

            var providerProximitySearchPostcodePage = _startPage.ClickShowAll();
            providerProximitySearchPostcodePage.AssertContent();
            providerProximitySearchPostcodePage.EnterPostcode("CV1 2WT");

            var providerProximityResultsPage = providerProximitySearchPostcodePage.ClickSearch();
            providerProximityResultsPage.AssertContent();

            providerProximityResultsPage.SearchCount().Should().Be(2);

            providerProximityResultsPage.SelectFilter(1);
            providerProximityResultsPage = providerProximityResultsPage.ClickFilterResults();
            Driver.Url.Should()
                .Be(
                    "https://localhost:5001/all-provider-results-CV1%202WT-Business%20and%20administration");
            providerProximityResultsPage.SearchCount().Should().Be(0);

            providerProximityResultsPage.SelectFilter(0);
            providerProximityResultsPage = providerProximityResultsPage.ClickFilterResults();
            Driver.Url.Should()
                .Be(
                    "https://localhost:5001/all-provider-results-CV1%202WT-Agriculture,%20environmental%20and%20animal%20care-Business%20and%20administration");
            providerProximityResultsPage.SearchCount().Should().Be(2);

            providerProximityResultsPage = providerProximityResultsPage.ClickFilterRemove();
            Driver.Url.Should().Be("https://localhost:5001/all-provider-results-CV1%202WT");

            var startPage = providerProximityResultsPage.ClickFinish();
            startPage.AssertContent();
        }

        public void Dispose()
        {
            Driver.Dispose();
            Driver.Quit();
        }
    }
}