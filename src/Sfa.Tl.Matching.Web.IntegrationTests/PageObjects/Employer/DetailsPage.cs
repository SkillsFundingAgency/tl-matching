using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class DetailsPage : PageBase
    {
        private readonly By _findDifferentLink = By.Id("tl-find-different");
        private readonly By _contactName = By.Id("EmployerContact");
        private readonly By _contactEmail = By.Id("EmployerContactEmail");
        private readonly By _contactPhone= By.Id("EmployerContactPhone");
        private readonly By _confirmButton = By.Id("tl-confirm");

        private const string Title = "Confirm contact details for industry placements";

        public DetailsPage(IWebDriver driver) : base(driver)
        {
        }

        public FindEmployerPage ClickFindDifferentLink()
        {
            Driver.FindElement(_findDifferentLink).Click();

            return new FindEmployerPage(Driver);
        }

        public CheckAnswersPage ClickConfirm()
        {
            Driver.FindElement(_confirmButton).Click();

            return new CheckAnswersPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(Title);
        }
    }
}