using OpenQA.Selenium;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.ProviderProximity
{
    public class ProviderProximityResultsPage : PageBase
    {
        private readonly By _filterButton = By.Id("tl-filter");
        private readonly By _filterRemoveLink = By.Id("tl-filter-remove");
        private readonly By _finishButton = By.Id("tl-finish");
        private readonly By _searchCount = By.Id("tl-search-count");

        private const string Title = "All providers in an area";

        public ProviderProximityResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public ProviderProximityResultsPage ClickFilterResults()
        {
            Driver.FindElement(_filterButton).Click();

            return new ProviderProximityResultsPage(Driver);
        }

        public void SelectFilter(int index)
        {
            Driver.FindElement(By.Id($"Filters_{index}__IsSelected")).Click();
        }

        public ProviderProximityResultsPage ClickFilterRemove()
        {
            Driver.FindElement(_filterRemoveLink).Click();

            return new ProviderProximityResultsPage(Driver);
        }

        public StartPage ClickFinish()
        {
            Driver.FindElement(_finishButton).Click();

            return new StartPage(Driver);
        }

        public int SearchCount()
        {
            return int.Parse(Driver.FindElement(_searchCount).Text);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1("Providers within 30 miles of CV1 2WT");
        }
    }
}