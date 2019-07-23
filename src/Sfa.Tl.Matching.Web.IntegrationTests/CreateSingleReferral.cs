using System.Threading;
using OpenQA.Selenium;
using Sfa.Tl.Matching.Web.IntegrationTests.PageObjects;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class CreateSingleReferral : IClassFixture<DotNetChromeFixture>
    {
        private readonly StartPage _startPage;

        public CreateSingleReferral(DotNetChromeFixture dotNetChromeFixture)
        {
            var idamsLogin = new IdamsLogin(dotNetChromeFixture.WebDriver);
            Thread.Sleep(5000);
            _startPage = idamsLogin.ClickSignInAsAdmin();
        }

        [Fact(DisplayName = "Create Single Referral")]
        public void CreateReferral()
        {
            _startPage.AssertContent();

            var proximityIndexPage = _startPage.ClickStart();
            proximityIndexPage.AssertContent();
            proximityIndexPage.EnterPostcode("SW1A 2AA");

            var proximityResultsPage = proximityIndexPage.ClickSearch();
            proximityResultsPage.AssertContent();
            proximityResultsPage.SelectProvider();
            proximityResultsPage.EnterSearchRadius("25 miles");

            var placementInformationPage = proximityResultsPage.ClickContinue();
            placementInformationPage.AssertContent();
            placementInformationPage = placementInformationPage.SelectPlacementsKnown();

            var findEmployerPage = placementInformationPage.ClickContinue();
            findEmployerPage.AssertContent();
            findEmployerPage = findEmployerPage.EnterCompanyName("Cafe Lilli");

            var detailsPage = findEmployerPage.ClickContinue();
            detailsPage.AssertContent();

            var checkAnswersPage = detailsPage.ClickConfirm();
            checkAnswersPage.AssertContent();
            checkAnswersPage.AssertDatabase();

            var opportunityBasketPage = checkAnswersPage.ClickConfirm();
            opportunityBasketPage.AssertContent();
            opportunityBasketPage.AssertDatabase();

            var employerConsentPage = opportunityBasketPage.ClickContinue();
            employerConsentPage.AssertContent();
            employerConsentPage = employerConsentPage.SelectConfirmationSelected();

            var referralEmailSentPage = employerConsentPage.ClickConfirm();
            referralEmailSentPage.AssertContent();
        }
    }
}