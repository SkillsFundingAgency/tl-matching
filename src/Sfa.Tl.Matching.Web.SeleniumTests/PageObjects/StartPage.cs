using FluentAssertions;
using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.OpportunityProximity;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.ProviderProximity;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects
{
    public class StartPage : PageBase
    {
        private readonly By _viewOpportunitiesLink = By.Id("tl-dash-viewsaved");
        private readonly By _uploadLink = By.Id("tl-dash-uploaddata");
        private readonly By _providerLink = By.Id("tl-dash-manageprovider");
        private readonly By _qualificationLink = By.Id("tl-dash-editquals");
        private readonly By _startButton = By.Id("tl-dash-createnew");
        private readonly By _showAllLink = By.Id("tl-dash-showall");
        
        public StartPage(IWebDriver driver) : base(driver)
        {
        }

        public void AssertContent()
        {
            //Thread.Sleep(10000);
            Driver.Title.Should().Be("Match employers with providers for industry placements - GOV.UK");

            //var uploadLink = _wait.Until(d => d.FindElement(_uploadLink));

            var uploadLink = Driver.FindElement(_uploadLink);
            uploadLink.Text.Should().Be("Manually upload data");
            var uploadPathAndQuery = new System.Uri(uploadLink.GetAttribute("href")).PathAndQuery;
            uploadPathAndQuery.Should().Be("/DataImport");

            var providerLink = Driver.FindElement(_providerLink);
            providerLink.Text.Should().Be("Manage provider data");
            var providerPathAndQuery = new System.Uri(providerLink.GetAttribute("href")).PathAndQuery;
            providerPathAndQuery.Should().Be("/search-ukprn");

            var qualificationLink = Driver.FindElement(_qualificationLink);
            qualificationLink.Text.Should().Be("Edit qualifications");
            var qualificationPathAndQuery = new System.Uri(qualificationLink.GetAttribute("href")).PathAndQuery;
            qualificationPathAndQuery.Should().Be("/edit-qualifications");
        }

        public OpportunityProximityIndexPage ClickStart()
        {
            Driver.FindElement(_startButton).Click();

            return new OpportunityProximityIndexPage(Driver);
        }

        public void ClickOpportunitiesLink()
        {
            Driver.FindElement(_viewOpportunitiesLink).Click();
        }

        public void ClickUploadLink()
        {
            Driver.FindElement(_uploadLink).Click();
        }

        public void ClickProviderLink()
        {
            Driver.FindElement(_providerLink).Click();
        }

        public ProviderProximitySearchPostcodePage ClickShowAll()
        {
            Driver.FindElement(_showAllLink).Click();

            return new ProviderProximitySearchPostcodePage(Driver);
        }

        public void ClickQualificationLink()
        {
            Driver.FindElement(_qualificationLink).Click();
        }
    }
}