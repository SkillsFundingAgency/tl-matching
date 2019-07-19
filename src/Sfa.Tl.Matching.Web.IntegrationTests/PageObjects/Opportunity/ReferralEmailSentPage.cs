using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class ReferralEmailSentPage : PageBase
    {
        private const string Title = "Emails sent";

        public ReferralEmailSentPage(IWebDriver driver) : base(driver)
        {
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(Title);
        }
    }
}