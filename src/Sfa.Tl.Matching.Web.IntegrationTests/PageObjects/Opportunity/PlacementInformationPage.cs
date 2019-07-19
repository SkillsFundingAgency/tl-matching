using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class PlacementInformationPage : PageBase
    {
        private readonly By _jobRole = By.Id("JobRole");
        private readonly By _placementsKnownYes = By.Id("placement-location-yes");
        private readonly By _placementsKnownNo = By.Id("placement-location-no");
        private readonly By _placements = By.Id("Placements");
        private readonly By _continueButton = By.Id("tl-continue");

        public PlacementInformationPage(IWebDriver driver) : base(driver)
        {
        }

        public FindEmployerPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new FindEmployerPage(Driver);
        }
    }
}