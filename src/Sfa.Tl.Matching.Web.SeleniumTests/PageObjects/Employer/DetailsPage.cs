using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Employer
{
    public class DetailsPage : PageBase, IPage
    {
        private readonly By _findDifferentLink = By.Id("tl-find-different");
        private readonly By _contactName = By.Id("EmployerContact");
        private readonly By _contactEmail = By.Id("EmployerContactEmail");
        private readonly By _contactPhone= By.Id("EmployerContactPhone");
        private readonly By _confirmButton = By.Id("tl-confirm");

        private const string Title = "Confirm contact details for industry placements - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "Confirm that the employer’s contact details for industry placements are accurate and up-to-date";

        public DetailsPage(IWebDriver driver) : base(driver)
        {
        }

        public FindEmployerPage ClickFindDifferentLink()
        {
            Driver.FindElement(_findDifferentLink).Click();

            return new FindEmployerPage(Driver);
        }

        public DetailsPage EnterContactName(string contactName)
        {
            Driver.FindElement(_contactName).SendKeys(contactName);

            return this;
        }

        public DetailsPage EnterEmail(string email)
        {
            Driver.FindElement(_contactEmail).SendKeys(email);

            return this;
        }

        public DetailsPage EnterPhone(string phone)
        {
            Driver.FindElement(_contactPhone).SendKeys(phone);

            return this;
        }

        public CheckAnswersPage ClickConfirm()
        {
            Driver.FindElement(_confirmButton).Click();

            return new CheckAnswersPage(Driver);
        }

        public OpportunityBasketPage ClickConfirmToOpprunityBasket()
        {
            Driver.FindElement(_confirmButton).Click();

            return new OpportunityBasketPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(HeaderText);
        }
    }
}