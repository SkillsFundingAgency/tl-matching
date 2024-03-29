﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.Opportunity;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.OpportunityProximity
{
    public class OpportunityProximityResultsPage : PageBase
    {
        private readonly By _noSuitableProvidersLink = By.Id("tl-search-nosuitable");
        private readonly By _skillAreaSelect = By.Id("SelectedRouteId");
        private readonly By _postcode = By.Id("Postcode");
        private readonly By _searchButton = By.Id("tl-search");
        private readonly By _firstProvider = By.Id("SearchResults_Results_0__IsSelected");
        private readonly By _continueButton = By.Id("tl-continue");

        private const string Title = "Select providers for this opportunity - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "Select providers for this opportunity";
        
        public OpportunityProximityResultsPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterSkillArea(string skillArea)
        {
            var element = Driver.FindElement(_skillAreaSelect);
            var selectElement = new SelectElement(element);
            selectElement.SelectByText(skillArea);
        }

        public void EnterPostcode(string postcode)
        {
            Driver.FindElement(_postcode).SendKeys(postcode);
        }

        public OpportunityProximityResultsPage ClickSearchAgain()
        {
            Driver.FindElement(_searchButton).Click();

            return new OpportunityProximityResultsPage(Driver);
        }

        public PlacementInformationPage ClickNoSuitableProvidersLink()
        {
            Driver.FindElement(_noSuitableProvidersLink).Click();

            return new PlacementInformationPage(Driver);
        }

        public PlacementInformationPage ClickContinue()
        {
            Driver.FindElement(_continueButton).Click();

            return new PlacementInformationPage(Driver);
        }

        public void SelectProvider()
        {
            Driver.FindElement(_firstProvider).Click();
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(HeaderText);
        }
    }
}