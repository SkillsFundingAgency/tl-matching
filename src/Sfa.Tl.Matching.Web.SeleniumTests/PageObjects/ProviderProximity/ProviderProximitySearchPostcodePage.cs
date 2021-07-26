using OpenQA.Selenium;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.ProviderProximity
{
    public class ProviderProximitySearchPostcodePage : PageBase
    {
        private readonly By _postcode = By.Id("Postcode");
        private readonly By _searchButton = By.Id("tl-search");

        private const string Title = "Where is the employer? - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "Where is the employer?";

        public ProviderProximitySearchPostcodePage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterPostcode(string postcode)
        {
            Driver.FindElement(_postcode).SendKeys(postcode);
        }

        public ProviderProximityResultsPage ClickSearch()
        {
            Driver.FindElement(_searchButton).Click();

            return new ProviderProximityResultsPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(HeaderText);
        }
    }
}