using FluentAssertions;
using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects
{
    public class PageBase
    {
        protected readonly IWebDriver Driver;

        private readonly By _header1 = By.TagName("h1");

        protected PageBase(IWebDriver driver)
        {
            Driver = driver;
        }

        protected void AssertTitle(string title)
        {
            Driver.Title.Should().Be($"{title}");
        }

        protected void AssertHeader1(string header1)
        {
            Driver.FindElement(_header1).Text.Should().Be(header1);
        }
    }
}