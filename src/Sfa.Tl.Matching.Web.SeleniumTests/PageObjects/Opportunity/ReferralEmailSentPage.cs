using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity
{
    public class ReferralEmailSentPage : PageBase, IPage
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