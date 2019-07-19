using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class EmployerConsentPage : PageBase
    {
        private readonly By _confirmationCheckBox = By.Id("ConfirmationSelected");
        private readonly By _confirmButton = By.Id("tl-confirm");

        public EmployerConsentPage(IWebDriver driver) : base(driver)
        {
        }
    }
}