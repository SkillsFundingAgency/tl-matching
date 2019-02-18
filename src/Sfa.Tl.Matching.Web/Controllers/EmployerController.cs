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

        public IActionResult Index()
        {
            return View("Name");
        }
        
        [HttpPost]
        public IActionResult Index(EmployerNameViewModel viewModel)
        {
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Details()
        {
            return View(nameof(Details));
        }
    }
}