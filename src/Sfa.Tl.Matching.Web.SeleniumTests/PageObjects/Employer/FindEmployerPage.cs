using System.Threading;
using OpenQA.Selenium;

// ReSharper disable UnusedMember.Local

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Employer
{
    public class FindEmployerPage : PageBase, IPage
    {
        private readonly By _companyName = By.Id("CompanyName");
        private readonly By _continueButton = By.Id("tl-continue");

        private const string Title = "Who is the employer?";

        public FindEmployerPage(IWebDriver driver) : base(driver)
        {
        }

        public FindEmployerPage EnterCompanyName(string companyName)
        {
            Driver.FindElement(_companyName).SendKeys(companyName);
            Thread.Sleep(2000);
            
            return this;
        }

        public DetailsPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new DetailsPage(Driver);
        }

        public void AssertContent()
        {
            // TODO FIX
            //AssertTitle(Title);
            //AssertHeader1(Title);
        }
    }
}