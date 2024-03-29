﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.SeleniumTests.PageObjects.OpportunityProximity
{
    public class OpportunityProximityIndexPage : PageBase, IPage
    {
        private readonly By _skillAreaSelect = By.Id("SelectedRouteId");
        private readonly By _postcode = By.Id("Postcode");
        private readonly By _searchButton = By.Id("tl-search");

        private const string Title = "Set up placement opportunity - Match employers with providers for industry placements - GOV.UK";
        private const string HeaderText = "Set up placement opportunity";

        public OpportunityProximityIndexPage(IWebDriver driver) : base(driver)
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

        public OpportunityProximityResultsPage ClickSearch()
        {
            Driver.FindElement(_searchButton).Click();

            return new OpportunityProximityResultsPage(Driver);
        }

        public void AssertContent()
        {
            AssertTitle(Title);
            AssertHeader1(HeaderText);
        }
    }
}