using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class FindEmployerPage : PageBase
    {
        private readonly By _companyName = By.Id("CompanyName");
        private readonly By _continueButton = By.Id("tl-continue");

        public FindEmployerPage(IWebDriver driver) : base(driver)
        {
        }
    }
}