using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Proximity;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class CheckAnswersPage : PageBase
    {
        private readonly By _skillAreaChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[1]/td[2]/a");
        private readonly By _postcodeChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[2]/td[2]/a");
        private readonly By _jobRoleChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[3]/td[2]/a");
        private readonly By _studentsWantedChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[4]/td[2]/a");
        private readonly By _confirmButton = By.Id("tl-confirm");

        public CheckAnswersPage(IWebDriver driver) : base(driver)
        {
        }

        public ProximityResultsPage ClickSkillAreaChangeLink()
        {
            Driver.FindElement(_skillAreaChangeLink).Click();

            return new ProximityResultsPage(Driver);
        }

        public ProximityResultsPage ClickPostcodeChangeLink()
        {
            Driver.FindElement(_postcodeChangeLink).Click();

            return new ProximityResultsPage(Driver);
        }

        public PlacementInformationPage ClickJobRoleChangeLink()
        {
            Driver.FindElement(_jobRoleChangeLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public PlacementInformationPage ClickStudentsWantedChangeLink()
        {
            Driver.FindElement(_studentsWantedChangeLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public OpportunityBasketPage ClickConfirm()
        {
            Driver.FindElement(_confirmButton).Click();

            return new OpportunityBasketPage(Driver);
        }
    }
}