using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class EmployerConsentPage : PageBase
    {
        private readonly By _contactDetailsChangeLink = By.XPath("//*[@id='tl-change-employer']");
        private readonly By _confirmationCheckBox = By.Id("ConfirmationSelected");
        private readonly By _confirmButton = By.Id("tl-confirm");

        private const string Title = "Confirm that we can share the employer’s contact details";

        public EmployerConsentPage(IWebDriver driver) : base(driver)
        {
        }

        public void ClickContactDetailsChangeLink()
        {
            Driver.FindElement(_contactDetailsChangeLink).Click();
        }

        public EmployerConsentPage SelectConfirmationSelected()
        {
            Driver.FindElement(_confirmationCheckBox).Click();
            return this;
        }

        public ReferralEmailSentPage ClickConfirm()
        {
            Driver.FindElement(_confirmButton).Click();

            return new ReferralEmailSentPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1("Before you send emails");
        }
    }
}