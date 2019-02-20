using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Constants;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class OpportunityModel
    {
        //[FromRoute]
        public int OpportunityId { get; set; }
    }

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

        [HttpGet]
        [Route(RouteTemplates.PlacementInformation)]
        public async Task<IActionResult> Placements(OpportunityModel opportunityViewModel)
        {
            var opportunity = await _opportunityService.GetOpportunity(opportunityViewModel.OpportunityId);
            var viewModel = new PlacementInformationViewModel
            {
                OpportunityId = opportunity.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route(RouteTemplates.PlacementInformation)]
        public async Task<IActionResult> Placements(PlacementInformationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.JobTitle = viewModel.JobTitle;
            dto.PlacementsKnown = viewModel.PlacementsKnown;
            dto.Placements = viewModel.Placements;

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute(RouteNames.EmployerNameGet, new OpportunityModel
            {
                OpportunityId = dto.Id
            });
        }
    }
}