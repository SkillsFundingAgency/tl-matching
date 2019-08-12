using OpenQA.Selenium;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects
{
    public class IdamsLogin
    {
        private const string StandardUsername = "Tmatching1";
        private const string StandardPassword = "";
        private const string AdminUsername = "Tmatching3";
        private const string AdminPassword = "";

        private readonly IWebDriver _driver;

        private readonly By _usernameTextBox = By.Id("username");
        private readonly By _passwordTextBox = By.Id("password");
        private readonly By _signInButton = By.XPath("//*[@id='mainContent']/div[2]/div[2]/form/div[5]/div/button");

        public IdamsLogin(IWebDriver driver)
        {
            _driver = driver;
        }

        public void EnterUsername(string username)
        {
            _driver.FindElement(_usernameTextBox).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            _driver.FindElement(_passwordTextBox).SendKeys(password);
        }

        public StartPage ClickSignIn()
        {
            _driver.FindElement(_signInButton).Click();

            return new StartPage(_driver);
        }

        public StartPage ClickSignInAsStandard()
        {
            EnterUsername(StandardUsername);
            EnterPassword(StandardPassword);

            return ClickSignIn();
        }

        public StartPage ClickSignInAsAdmin()
        {
            EnterUsername(AdminUsername);
            EnterPassword(AdminPassword);

            return ClickSignIn();
        }

        public StartPage ClickSignInAs(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);

            return ClickSignIn();
        }
    }
}