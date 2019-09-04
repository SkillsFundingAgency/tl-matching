using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class NavigationController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IBackLinkService _backLinkService;
        
        public NavigationController(IOpportunityService opportunityService, IBackLinkService backLinkService)
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
            var viewModel = await _opportunityService.GetCheckAnswers(opportunityItemId);
            var opportunities = await _opportunityService.GetOpportunityBasket(viewModel.OpportunityId);

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
            var viewModel = await _opportunityService.GetCheckAnswers(opportunityItemId);
            var opportunities = await _opportunityService.GetOpportunityBasket(viewModel.OpportunityId);

            if (opportunities.ReferralCount == 0 && opportunities.ProvisionGapCount == 1)
            {
                return RedirectToRoute("GetEmployerDetails",
                    new { opportunityId = viewModel.OpportunityId, opportunityItemId });
            }

            return RedirectToRoute("GetCheckAnswers", new { viewModel.OpportunityItemId});
        }

        [HttpGet]
        [Route("save-employer-opportunity/{opportunityId}", Name = "SaveEmployerOpportunity")]
        public async Task<IActionResult> SaveEmployerOpportunity(int opportunityId)
        {
            await _opportunityService.ClearOpportunityItemsSelectedForReferralAsync(opportunityId);

            return RedirectToRoute("GetSavedEmployerOpportunity");
        }

        [HttpGet]
        [Route("get-back-link", Name = "GetBackLink")]
        public async Task<IActionResult> BackLink()
        {
            return Redirect(await _backLinkService.GetBackLink(HttpContext.User.GetUserName()));
        }
    }
}