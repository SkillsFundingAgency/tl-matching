using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Proximity
{
    public class ProximityResultsPage : PageBase
    {
        private readonly By _noSuitableProvidersLink = By.Id("tl-search-nosuitable");
        private readonly By _skillAreaSelect = By.Id("SelectedRouteId");
        private readonly By _postcode = By.Id("Postcode");
        private readonly By _searchRadiusSelect = By.Id("SearchRadius");
        private readonly By _searchButton = By.Id("tl-search");
        private readonly By _continueButton = By.Id("tl-continue");

        public ProximityResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterSkillArea(string skillArea)
        {
            var element = Driver.FindElement(_skillAreaSelect);
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(skillArea);
        }

        public void EnterPostcode(string postcode)
        {
            Driver.FindElement(_postcode).SendKeys(postcode);
        }

        public void EnterSearchRadius(string radius)
        {
            var element = Driver.FindElement(_searchRadiusSelect);
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(radius);
        }

        public ProximityResultsPage ClickSearchAgain()
        {
            Driver.FindElement(_searchButton).Click();

            return new ProximityResultsPage(Driver);
        }

        public PlacementInformationPage ClickNoSuitableProvidersLink()
        {
            Driver.FindElement(_noSuitableProvidersLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public ProximityResultsPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new ProximityResultsPage(Driver);
        }

        public void AssertContent()
        {
            var skillAreaDropdown = Driver.FindElement(_skillAreaSelect);
            var skillAreaSelect = new SelectElement(skillAreaDropdown);
            skillAreaSelect.AllSelectedOptions.Count.Should().BeGreaterThan(1);
        }
    }
}