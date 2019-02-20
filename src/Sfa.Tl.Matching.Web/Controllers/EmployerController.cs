using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class EmployerController : Controller
    {
        public EmployerController()
        {
        }

        [HttpGet("who-is-employer", Name = RouteNames.EmployerNameGet)]
        public IActionResult Name()
        {
            return View();
        }
        
        [HttpPost("who-is-employer", Name = RouteNames.EmployerNamePost)]
        public IActionResult Name(EmployerNameViewModel viewModel)
        {
            return RedirectToAction(nameof(Details));
        }

        [HttpGet("employer-details", Name = RouteNames.EmployerDetailsGet)]
        public IActionResult Details()
        {
            return View();
        }


        public IActionResult Check()
        {
            return View();
        }
    }
}
