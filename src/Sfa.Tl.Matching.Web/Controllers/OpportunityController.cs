using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IOpportunityService _opportunityService;

        public OpportunityController(IMapper mapper, IOpportunityService opportunityService)
        {
            _mapper = mapper;
            _opportunityService = opportunityService;
        }

        [HttpPost]
        [Route("opportunity-create", Name = "OpportunityCreate_Post")]
        public async Task<IActionResult> Create(OpportunityDto dto)
        {
            // TODO AU REMOVE
            dto.Postcode = "AA1 1AA";
            dto.Distance = 1;
            dto.RouteId = 1;

            var opportunityId = await _opportunityService.CreateOpportunity(dto);

            TempData["OpportunityId"] = opportunityId;

            return RedirectToRoute("Placements_Get");
        }

        [HttpGet]
        [Route("placement-information", Name = "Placements_Get")]
        public IActionResult Placements()
        {
            var opportunityId = (int)TempData["OpportunityId"];

            var viewModel = new PlacementInformationViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information", Name = "Placements_Post")]
        public async Task<IActionResult> Placements(PlacementInformationViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.JobTitle = viewModel.JobTitle;
            dto.PlacementsKnown = viewModel.PlacementsKnown;
            dto.Placements = viewModel.Placements;

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("EmployerName_Get");
        }

        private void Validate(PlacementInformationViewModel viewModel)
        {
            if (!viewModel.PlacementsKnown) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many placements the employer wants at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must enter a number that is 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must enter a number that is 999 or less");
        }
    }
}