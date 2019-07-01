using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
#endif
    public class NavigationController : Controller
    {
        private readonly IOpportunityService _opportunityService;

        public NavigationController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [HttpGet]
        [Route("remove-opportunityItem/{opportunityId}-{opportunityItemId}", Name = "RemoveAndGetOpportunityBasket")]
        public async Task<IActionResult> RemoveOpportunityItemAndGetOpportunityBasket(int opportunityId, int opportunityItemId)
        {
            await _opportunityService.RemoveOpportunityItemAsync(opportunityId, opportunityItemId);
            var opportunityItemCount = await _opportunityService.GetOpportunityItemCountAsync(opportunityId);

            return opportunityItemCount == 0
                ? RedirectToRoute("Start")
                : RedirectToRoute("GetOpportunityBasket", new { opportunityId, opportunityItemId });
        }

        [HttpGet]
        [Route("get-placement-or-employer/{opportunityId}-{opportunityItemId}", Name = "GetPlacementOrEmployer")]
        public async Task<IActionResult> GetPlacementOrEmployer(int opportunityId, int opportunityItemId)
        {
            var opportunityItemCount = await _opportunityService.GetOpportunityItemCountAsync(opportunityId);

            return opportunityItemCount == 0
                ? RedirectToRoute("GetEmployerDetails", new { opportunityId, opportunityItemId })
                : RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
        }

        [HttpGet]
        [Route("check-answers-or-edit-employer/{opportunityItemId}", Name = "GetCheckAnswersOrEditEmployer")]
        public async Task<IActionResult> GetCheckAnswersOrEditEmployer(int opportunityItemId)
        {
            var checkAnswersViewModel = await GetCheckAnswersViewModel(opportunityItemId);
            var opportunities = await _opportunityService.GetOpportunityBasket(checkAnswersViewModel.OpportunityId);

            if (opportunities.ReferralCount == 0 && opportunities.ProvisionGapCount == 1)
            {
                return RedirectToRoute("GetEmployerDetails",
                    new { opportunityId = checkAnswersViewModel.OpportunityId, opportunityItemId = opportunityItemId });
            }

            return RedirectToRoute("GetCheckAnswers", new { checkAnswersViewModel .OpportunityItemId});

        }

        private async Task<CheckAnswersViewModel> GetCheckAnswersViewModel(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetCheckAnswers(opportunityItemId);

            return viewModel;
        }
    }
}