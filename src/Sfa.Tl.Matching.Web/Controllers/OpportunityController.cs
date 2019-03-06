using System.Collections.Generic;
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

        [Route("opportunity-within-{distance}-miles-of-{postcode}-for-route-{routeId}", Name = "OpportunityCreate_Get")]
        public async Task<IActionResult> Create(int routeId, string postcode, short distance)
        {
            var dto = new OpportunityDto
            {
                RouteId = routeId,
                Postcode = postcode,
                Distance = distance,
                IsReferral = false,  // TODO AU FIX THIS
                CreatedBy = HttpContext.User.GetUserName(),
                UserEmail = HttpContext.User.GetUserEmail()
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
        [Route("placement-information", Name = "Placements_Post")]
        public async Task<IActionResult> Placements(PlacementInformationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.SavePlacementInformation(viewModel);

            return RedirectToRoute("EmployerFind_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "CheckAnswers_Get")]
        public async Task<IActionResult> CheckAnswers(int id)
        {
            var viewModel = await GetCheckAnswersViewModel(id);

            return View(viewModel);
        }

        private async Task<CheckAnswersViewModel> GetCheckAnswersViewModel(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersViewModel
            {
                OpportunityId = dto.Id,
                Contact = dto.EmployerContact,
                Distance = dto.Distance,
                EmployerName = dto.EmployerName,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown,
                Placements = dto.Placements,
                Postcode = dto.Postcode,
                Route = dto.Route
            };

            viewModel.Providers = new List<ProviderViewModel>();
            viewModel.Providers.Add(new ProviderViewModel
            {
                Name = "The WKCIC Group1111",
                Distance = 2,
                Postcode = "CV1 2WT",
            });

            return viewModel;
        }

        [HttpPost]
        [Route("check-answers", Name = "CheckAnswers_Post")]
        public async Task<IActionResult> CheckAnswers(CheckAnswersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersViewModel(viewModel.OpportunityId));

            viewModel.CreatedBy = HttpContext.User.GetUserName();

            //await _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToAction(nameof(PlacementGap), new { opportunityId = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersGap_Get")]
        public async Task<IActionResult> CheckAnswersGap(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersGapViewModel
            {
                OpportunityId = dto.Id,
                Contact = dto.EmployerContact,
                Distance = dto.Distance,
                EmployerName = dto.EmployerName,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown,
                Placements = dto.Placements,
                Postcode = dto.Postcode,
                Route = dto.Route
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers-gap", Name = "CheckAnswersGap_Post")]
        public async Task<IActionResult> CheckAnswersGap(CheckAnswersGapViewModel viewModel)
        {
            viewModel.CreatedBy = HttpContext.User.GetUserName();

            await _opportunityService.CreateProvisionGap(viewModel);

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