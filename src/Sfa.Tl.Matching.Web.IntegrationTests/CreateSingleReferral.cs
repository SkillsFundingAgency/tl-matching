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
            _startPage = idamsLogin.ClickSignInAsAdmin();
        }

        [Fact(DisplayName = "Create Single Referral")]
        public void CreateReferral1()
        {
            _startPage.AssertContent();

            var proximityIndexPage = _startPage.ClickStart();
            proximityIndexPage.AssertContent();
            proximityIndexPage.EnterPostcode("SW1A 2AA");

            var proximityResultsPage = proximityIndexPage.ClickSearch();
            proximityResultsPage.AssertContent();
            proximityResultsPage.SelectProvider();

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

            var opportunityBasketPage = checkAnswersPage.ClickConfirm();
            opportunityBasketPage.AssertContent();

            var employerConsentPage = opportunityBasketPage.ClickContinue();
            employerConsentPage.AssertContent();
            employerConsentPage = employerConsentPage.SelectConfirmationSelected();

            var referralEmailSentPage = employerConsentPage.ClickConfirm();
            referralEmailSentPage.AssertContent();
        }
    }
}