using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Employer;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity
{
    public class OpportunityBasketPage : PageBase, IPage
    {
        private readonly By _addOpportunityLink = By.CssSelector("govuk-button tl-button--grey");
        private readonly By _editLink = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[5]/a");
        private readonly By _deleteLink = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[6]/a");
        private readonly By _continueButton = By.Id("tl-continue");
        private readonly By _downloadSpreadsheetLink = By.Id("tl-download");
        private readonly By _finishButton = By.Id("tl-finish");

        private readonly By _workplaceValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[1]");
        private readonly By _jobRoleValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[2]");
        private readonly By _studentsWantedValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[3]");
        private readonly By _providersValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[4]");

        private const string Title = "All opportunities - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "All opportunities";

        public OpportunityBasketPage(IWebDriver driver) : base(driver)
        {
        }

        public void ClickAddOpportunityLink()
        {
            Driver.FindElement(_addOpportunityLink).Click();
        }

        public StartPage ClickFinish()
        {
            Driver.FindElement(_finishButton).Click();

            return new StartPage(Driver);
        }

        public EmployerConsentPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new EmployerConsentPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(HeaderText);
        }
    }
}