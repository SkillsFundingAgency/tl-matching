using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects
{
    public class PageBase
    {
        protected readonly IWebDriver Driver;

        public PageBase(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}