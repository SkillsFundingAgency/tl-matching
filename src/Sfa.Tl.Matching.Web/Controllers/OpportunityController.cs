using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
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
        
        [HttpGet("placement-information", Name = RouteNames.PlacementsGet)]
        public IActionResult Placements()
        {
            var viewModel = new PlacementInformationViewModel();

            return View(viewModel);
        }

        [HttpPost("placement-information", Name = RouteNames.PlacementsPost)]
        public IActionResult Placements(PlacementInformationViewModel viewModel)
        {
            return RedirectToRoute(RouteNames.EmployerNameGet);
        }
    }
}