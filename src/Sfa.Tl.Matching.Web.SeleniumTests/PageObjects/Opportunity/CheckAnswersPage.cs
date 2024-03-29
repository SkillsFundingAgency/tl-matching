﻿using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.OpportunityProximity;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity
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
        
        private const string Title = "Check answers - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "Check answers";

        public CheckAnswersPage(IWebDriver driver) : base(driver)
        {
        }

        public OpportunityProximityResultsPage ClickSkillAreaChangeLink()
        {
            Driver.FindElement(_skillAreaChangeLink).Click();

            return new OpportunityProximityResultsPage(Driver);
        }

        public OpportunityProximityResultsPage ClickPostcodeChangeLink()
        {
            Driver.FindElement(_postcodeChangeLink).Click();

            return new OpportunityProximityResultsPage(Driver);
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

        public OpportunityProximityResultsPage ClickProvidersChangeLink()
        {
            Driver.FindElement(_providersChangeLink).Click();

            return new OpportunityProximityResultsPage(Driver);
        }

        public OpportunityBasketPage ClickConfirm()
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