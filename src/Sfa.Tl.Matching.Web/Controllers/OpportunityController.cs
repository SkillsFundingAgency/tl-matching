using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class OpportunityController : Controller
    {
        private readonly IOpportunityService _opportunityService;

        public OpportunityController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [Route("opportunity-within-{distance}-miles-of-{postcode}-for-route-{routeId}", Name = "OpportunityCreate_Post")]
        public async Task<IActionResult> Create(int routeId, string postcode, short distance)
        {
            var dto = new OpportunityDto
            {
                RouteId = routeId,
                Postcode = postcode,
                Distance = distance,
                CreatedBy = HttpContext.User.GetUserName()
            };

            var id = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("Placements_Get", new
            {
                id
            });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "Placements_Get")]
        public async Task<IActionResult> Placements(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);

            var viewModel = new PlacementInformationViewModel
            {
                RouteId = dto.RouteId,
                Postcode = dto.Postcode,
                Distance = dto.Distance,
                OpportunityId = dto.Id,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown,
                Placements = dto.Placements
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information/{id?}", Name = "Placements_Post")]
        public async Task<IActionResult> Placements(PlacementInformationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new OpportunityDto
            {
                Id = viewModel.OpportunityId,
                JobTitle = viewModel.JobTitle,
                PlacementsKnown = viewModel.PlacementsKnown,
                Placements = viewModel.Placements,
                ModifiedBy = HttpContext.User.GetUserName()
            };

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("EmployerFind_Get", new { dto.Id });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "CheckAnswers_Get")]
        public IActionResult CheckAnswers(int id)
        {
            return View(new CheckAnswersViewModel { OpportunityId = id });
        }

        [HttpPost]
        [Route("check-answers/{id?}", Name = "CheckAnswers_Post")]
        public IActionResult CheckAnswers(CheckAnswersViewModel viewModel)
        {
            viewModel.CreatedBy = HttpContext.User.GetUserName();

            _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToAction(nameof(PlacementGap), new { opportunityId = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap", Name = "PlacementGap_Get")]
        public async Task<IActionResult> PlacementGap(int opportunityId)
        {
            var opportunity = await _opportunityService.GetOpportunity(opportunityId);

            return View(new PlacementGapViewModel { EmployerContactName = opportunity.EmployerContact });
        }

        private void Validate(PlacementInformationViewModel viewModel)
        {
            if (!viewModel.PlacementsKnown.HasValue || !viewModel.PlacementsKnown.Value) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many placements the employer wants at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 999 or less");
        }
    }
}