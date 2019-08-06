using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Employer;

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity
{
    public class PlacementInformationPage : PageBase, IPage
    {
        private readonly By _jobRole = By.Id("JobRole");
        private readonly By _placementsKnownYes = By.Id("placement-location-yes");
        private readonly By _placementsKnownNo = By.Id("placement-location-no");
        private readonly By _placements = By.Id("Placements");
        private readonly By _continueButton = By.Id("tl-continue");

        private const string Title = "Placement information";

        public PlacementInformationPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterJobRole(string jobRole)
        {
            Driver.FindElement(_jobRole).SendKeys(jobRole);
        }

        public PlacementInformationPage SelectPlacementsKnown(bool? value = false)
        {
            if (value.HasValue && value.Value)
            {
                Driver.FindElement(_placementsKnownYes).Click();
                return this;
            }

            Driver.FindElement(_placementsKnownNo).Click();
            return this;
        }

        public void EnterPlacements(string placements)
        {
            Driver.FindElement(_placements).SendKeys(placements);
        }

        public FindEmployerPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new FindEmployerPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(Title);
        }

        public void AssertDatabase()
        {
            throw new System.NotImplementedException();
        }
    }
}