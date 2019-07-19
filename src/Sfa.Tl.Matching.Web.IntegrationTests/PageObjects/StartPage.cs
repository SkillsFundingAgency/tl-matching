using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Proximity;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects
{
    public class StartPage : PageBase
    {
        private readonly By _uploadLink = By.Id("tl-upload-link");
        private readonly By _providerLink = By.Id("tl-Add-edit-provider-link");
        private readonly By _qualificationLink = By.Id("tl-edit-qualifications-link");
        private readonly By _startButton = By.Id("tl-start-now");

        public StartPage(IWebDriver driver) : base(driver)
        {
        }

        public void AssertContent()
        {
            Thread.Sleep(10000);
            Driver.Title.Should().Be("Match employers with providers for industry placements - GOV.UK");

            //var uploadLink = _wait.Until(d => d.FindElement(_uploadLink));

            var uploadLink = Driver.FindElement(_uploadLink);
            uploadLink.Text.Should().Be("Upload employer and provider data");
            uploadLink.GetAttribute("href").Should().Be("https://localhost:55229/DataImport");

            var providerLink = Driver.FindElement(_providerLink);
            providerLink.Text.Should().Be("Add or edit provider data");
            providerLink.GetAttribute("href").Should().Be("https://localhost:55229/search-ukprn");

            var qualificationLink = Driver.FindElement(_qualificationLink);
            qualificationLink.Text.Should().Be("Edit qualifications");
            qualificationLink.GetAttribute("href").Should().Be("https://localhost:55229/edit-qualifications");
        }

        public ProximityIndexPage ClickStart()
        {
            Driver.FindElement(_startButton).Click();

            return new ProximityIndexPage(Driver);
        }
    }
}