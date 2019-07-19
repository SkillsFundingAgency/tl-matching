using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class CheckAnswersPage : PageBase
    {
        private readonly By _confirmButton = By.Id("tl-confirm");

        public CheckAnswersPage(IWebDriver driver) : base(driver)
        {
        }
    }
}