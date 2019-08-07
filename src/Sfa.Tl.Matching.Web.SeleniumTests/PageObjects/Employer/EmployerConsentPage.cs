using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity;

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Employer
{
    public class EmployerConsentPage : PageBase, IPage
    {
        private readonly By _contactDetailsChangeLink = By.XPath("//*[@id='tl-change-employer']");
        private readonly By _confirmationCheckBox = By.Id("ConfirmationSelected");
        private readonly By _continueButton = By.Id("tl-continue");

        private readonly By _contactNameValue = By.XPath("//*[@id='main-content']/div/div/section/p[1]");
        private readonly By _contactEmailValue = By.XPath("//*[@id='main-content']/div/div/section/p[2]");
        private readonly By _contactPhoneValue = By.XPath("//*[@id='main-content']/div/div/section/p[3]");

        private readonly By _confirmationValue =
            By.XPath("//*[@id='main-content']/div/div/form/div/fieldset/div/div/label");

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
            Driver.FindElement(_continueButton).Click();

            return new ReferralEmailSentPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1("Before you send emails");
        }
    }
}