using System.Linq;
using FluentAssertions;
using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.Database;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class OpportunityBasketPage : PageBase, IPage
    {
        private readonly By _addOpportunityLink = By.CssSelector("govuk-button tl-button--grey");
        private readonly By _editLink = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[5]/a");
        private readonly By _deleteLink = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[6]/a");
        private readonly By _continueButton = By.Id("tl-continue");
        private readonly By _downloadSpreadsheetLink = By.Id("tl-download");

        private readonly By _workplaceValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[1]");
        private readonly By _jobRoleValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[2]");
        private readonly By _studentsWantedValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[3]");
        private readonly By _providersValue = By.XPath("//*[@id='main-content']/div[2]/div/form/table/tbody/tr/td[4]");

        private const string Title = "All opportunities";

        public OpportunityBasketPage(IWebDriver driver) : base(driver)
        {
        }

        public void ClickAddOpportunityLink()
        {
            Driver.FindElement(_addOpportunityLink).Click();
        }

        public EmployerConsentPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new EmployerConsentPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(Title);
        }

        public void AssertDatabase()
        {
            var opportunityRetriever = new OpportunityRetriever();
            var opportunity = opportunityRetriever.GetLast();
            var opportunityItem = opportunity.OpportunityItem.First();

            var workplaceOnScreen = Driver.FindElement(_workplaceValue).Text;
            workplaceOnScreen.Should().Be($"{opportunityItem.Town} {opportunityItem.Postcode}");

            var jobRoleOnScreen = Driver.FindElement(_jobRoleValue).Text;
            jobRoleOnScreen.Should().Be(opportunityItem.JobRole);

            var studentsWantedOnScreen = Driver.FindElement(_studentsWantedValue).Text;
            studentsWantedOnScreen.Should().Be(opportunityItem.PlacementsKnown.GetValueOrDefault()
                ? opportunityItem.Placements.ToString()
                : "at least 1");

            var providersOnScreen = Driver.FindElement(_providersValue).Text;
            providersOnScreen.Should().Be(opportunityItem.Referral.Count.ToString());
        }
    }
}