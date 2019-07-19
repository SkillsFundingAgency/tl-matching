using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Proximity
{
    public class ProximityIndexPage : PageBase
    {
        private readonly By _skillAreaSelect = By.Id("SelectedRouteId");
        private readonly By _postcode = By.Id("Postcode");
        private readonly By _searchButton = By.Id("Search");

        public ProximityIndexPage(IWebDriver driver) : base(driver)
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

        public StartPage ClickSearch()
        {
            Driver.FindElement(_searchButton).Click();

            return new StartPage(Driver);
        }

        public void AssertContent()
        {
            var skillAreaDropdown = Driver.FindElement(_skillAreaSelect);
            var skillAreaSelect = new SelectElement(skillAreaDropdown);
            skillAreaSelect.AllSelectedOptions.Count.Should().BeGreaterThan(1);
        }
    }
}