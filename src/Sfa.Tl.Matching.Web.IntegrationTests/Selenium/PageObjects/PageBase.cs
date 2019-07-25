using FluentAssertions;
using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Selenium.PageObjects
{
    public class PageBase
    {
        protected readonly IWebDriver Driver;

        private readonly By _header1 = By.TagName("h1");

        public PageBase(IWebDriver driver)
        {
            Driver = driver;
        }

        public void AssertTitle(string title)
        {
            Driver.Title.Should().Be($"{title} - {Constants.ServiceName} - GOV.UK");
        }

        public void AssertHeader1(string header1)
        {
            Driver.FindElement(_header1).Text.Should().Be(header1);
        }
    }
}