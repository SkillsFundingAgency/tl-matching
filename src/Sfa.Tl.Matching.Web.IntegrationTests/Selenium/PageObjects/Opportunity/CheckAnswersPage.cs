using System.Linq;
using FluentAssertions;
using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.Database;
using Sfa.Tl.Matching.Web.IntegrationTests.Selenium.PageObjects.Proximity;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Selenium.PageObjects.Opportunity
{
    public class CheckAnswersPage : PageBase, IPage
    {
        private readonly By _skillAreaChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[1]/td[2]/a");
        private readonly By _postcodeChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[2]/td[2]/a");
        private readonly By _jobRoleChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[3]/td[2]/a");
        private readonly By _studentsWantedChangeLink = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[4]/td[2]/a");
        private readonly By _providersChangeLink = By.Id("tl-change-providers");
        private readonly By _confirmButton = By.Id("tl-confirm");

        private readonly By _skillAreaValue = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[1]/td[1]");
        private readonly By _postcodeValue = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[2]/td[1]");
        private readonly By _jobRoleValue = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[3]/td[1]");
        private readonly By _studentsWantedValue = By.XPath("//*[@id='main-content']/div/div/table/tbody/tr[4]/td[1]");
        private readonly By _provider1Value = By.XPath("//*[@id='main-content']/div/div/p[1]");
        private readonly By _provider1DistanceValue = By.XPath("//*[@id='main-content']/div/div/p[2]");
        
        private const string Title = "Check answers";

        public CheckAnswersPage(IWebDriver driver) : base(driver)
        {
        }

        public ProximityResultsPage ClickSkillAreaChangeLink()
        {
            Driver.FindElement(_skillAreaChangeLink).Click();

            return new ProximityResultsPage(Driver);
        }

        public ProximityResultsPage ClickPostcodeChangeLink()
        {
            Driver.FindElement(_postcodeChangeLink).Click();

            return new ProximityResultsPage(Driver);
        }

        public PlacementInformationPage ClickJobRoleChangeLink()
        {
            Driver.FindElement(_jobRoleChangeLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public PlacementInformationPage ClickStudentsWantedChangeLink()
        {
            Driver.FindElement(_studentsWantedChangeLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public ProximityResultsPage ClickProvidersChangeLink()
        {
            Driver.FindElement(_providersChangeLink).Click();

            return new ProximityResultsPage(Driver);
        }

        public OpportunityBasketPage ClickConfirm()
        {
            Driver.FindElement(_confirmButton).Click();

            return new OpportunityBasketPage(Driver);
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

            var skillAreaOnScreen = Driver.FindElement(_skillAreaValue).Text;
            skillAreaOnScreen.Should().Be(opportunityItem.Route.Name);

            var postcodeOnScreen = Driver.FindElement(_postcodeValue).Text;
            postcodeOnScreen.Should().Be(opportunityItem.Postcode);

            var jobRoleOnScreen = Driver.FindElement(_jobRoleValue).Text;
            jobRoleOnScreen.Should().Be(opportunityItem.JobRole);

            var studentsWantedOnScreen = Driver.FindElement(_studentsWantedValue).Text;
            studentsWantedOnScreen.Should().Be(opportunityItem.PlacementsKnown.GetValueOrDefault()
                ? opportunityItem.Placements.ToString()
                : "at least 1");

            var referral1 = opportunityItem.Referral.First();
            var providerVenue1 = referral1.ProviderVenue;
            var provider1 = providerVenue1.Provider;
            var provider1OnScreen = Driver.FindElement(_provider1Value).Text;
            provider1OnScreen.Should().Be(provider1.Name);

            var provider1DistanceOnScreen = Driver.FindElement(_provider1DistanceValue).Text;
            provider1DistanceOnScreen.Should()
                .Be(
                    $"{referral1.DistanceFromEmployer:#0.0} miles from {opportunityItem.Postcode}");
        }
    }
}