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

        [HttpPost]
        [Route("opportunity-create", Name = "OpportunityCreate_Post")]
        public async Task<IActionResult> Create(OpportunityDto dto)
        {
            dto.CreatedBy = HttpContext.User.GetUserName();

            var opportunityId = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("Placements_Get", new
            {
                OpportunityId = opportunityId
            });
        }

        [HttpGet]
        [Route("placement-information", Name = "Placements_Get")]
        public async Task<IActionResult> Placements(int opportunityId)
        {
            var dto = await _opportunityService.GetOpportunity(opportunityId);

            var viewModel = new PlacementInformationViewModel
            {
                OpportunityId = dto.Id,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown ?? false,
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

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.JobTitle = viewModel.JobTitle;
            dto.PlacementsKnown = viewModel.PlacementsKnown;
            dto.Placements = viewModel.Placements;
            dto.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("EmployerFind_Get", new
            {
                OpportunityId = dto.Id
            });
        }

        [HttpGet]
        [Route("check-answers", Name = "CheckAnswers_Get")]
        public IActionResult CheckAnswers(int opportunityId)
        {
            return View();
        }

        private void Validate(PlacementInformationViewModel viewModel)
        {
            if (!viewModel.PlacementsKnown.HasValue || !viewModel.PlacementsKnown.Value) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many placements the employer wants at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must enter a number that is 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must enter a number that is 999 or less");
        }
    }
}