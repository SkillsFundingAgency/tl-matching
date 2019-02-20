using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Constants;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class EmployerController : Controller
    {
        private readonly IOpportunityService _opportunityService;

        public EmployerController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [HttpGet]
        [Route(RouteTemplates.EmployerName, Name = RouteNames.EmployerNameGet)]
        public IActionResult Name(OpportunityModel opportunityModel)
        {
            return View();
        }

        [HttpPost]
        [Route(RouteTemplates.EmployerName, Name = RouteNames.EmployerNamePost)]
        public IActionResult Name(EmployerNameViewModel viewModel)
        {
            return RedirectToAction(nameof(Details));
        }

        [HttpGet]
        [Route(RouteTemplates.EmployerDetails, Name = RouteNames.EmployerDetailsGet)]
        public IActionResult Details()
        {
            return View();
        }
    }
}