using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class NavigationController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly INavigationService _backLinkService;

        public NavigationController(IOpportunityService opportunityService, INavigationService backLinkService)
        {
            _opportunityService = opportunityService;
            _backLinkService = backLinkService;
        }

        [HttpGet]
        [Route("remove-opportunityItem/{opportunityId}-{opportunityItemId}", Name = "RemoveAndGetOpportunityBasket")]
        public async Task<IActionResult> RemoveOpportunityItemAndGetOpportunityBasket(int opportunityId, int opportunityItemId)
        {
            await _opportunityService.DeleteOpportunityItemAsync(opportunityId, opportunityItemId);
            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId);

            return opportunityItemCount == 0
                ? RedirectToRoute("Start")
                : RedirectToRoute("GetOpportunityBasket", new { opportunityId, opportunityItemId });
        }

        [HttpGet]
        [Route("get-placement-or-employer/{opportunityId}-{opportunityItemId}", Name = "GetPlacementOrEmployer")]
        public async Task<IActionResult> GetPlacementOrEmployer(int opportunityId, int opportunityItemId)
        {
            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId);
            var viewModel = await _opportunityService.GetCheckAnswersAsync(opportunityItemId);
            var opportunities = await _opportunityService.GetOpportunityBasketAsync(viewModel.OpportunityId);

            switch (opportunities.ReferralCount)
            {
                case 0 when opportunityItemCount >= 1:
                    return RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
                case 0 when opportunities.ProvisionGapCount == 1:
                    return RedirectToRoute("GetEmployerDetails",
                        new { opportunityId = viewModel.OpportunityId, opportunityItemId });
                case 0 when opportunities.ProvisionGapCount == 0:
                    return RedirectToRoute("GetEmployerDetails",
                        new { opportunityId = viewModel.OpportunityId, opportunityItemId });
                default:
                    return RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
            }
        }

        [HttpGet]
        [Route("check-answers-or-edit-employer/{opportunityItemId}", Name = "GetCheckAnswersOrEditEmployer")]
        public async Task<IActionResult> GetCheckAnswersOrEditEmployer(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetCheckAnswersAsync(opportunityItemId);
            var opportunities = await _opportunityService.GetOpportunityBasketAsync(viewModel.OpportunityId);

            if (opportunities.ReferralCount == 0 && opportunities.ProvisionGapCount == 1)
            {
                return RedirectToRoute("GetEmployerDetails",
                    new { opportunityId = viewModel.OpportunityId, opportunityItemId });
            }

            return RedirectToRoute("GetCheckAnswers", new { viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("save-employer-opportunity/{opportunityId}", Name = "SaveEmployerOpportunity")]
        public async Task<IActionResult> SaveEmployerOpportunity(int opportunityId)
        {
            await _opportunityService.ClearOpportunityItemsSelectedForReferralAsync(opportunityId);

            return RedirectToRoute("GetSavedEmployerOpportunity");
        }

        [HttpGet]
        [Route("get-back-link/{OpportunityId}/{OpportunityItemId}/{Postcode}/{SelectedRouteId}", Name = "GetBackLink")]
        public async Task<IActionResult> BackLink(SearchParametersViewModel viewModel)
        {
            var prevUrl = await _backLinkService.GetBackLinkAsync(HttpContext.User.GetUserName());

            if (prevUrl.Contains("provider-results-for-opportunity") && viewModel.OpportunityId != 0)
            {
                await _backLinkService.GetBackLinkAsync(HttpContext.User.GetUserName());

                return RedirectToRoute("GetProviderResults", new SearchParametersViewModel
                {
                    SelectedRouteId = viewModel.SelectedRouteId,
                    Postcode = viewModel.Postcode,
                    OpportunityId = viewModel.OpportunityId,
                    OpportunityItemId = viewModel.OpportunityItemId,
                    CompanyNameWithAka = viewModel.CompanyNameWithAka
                });
            }

            return Redirect(prevUrl);
        }

    }
}