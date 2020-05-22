using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public HomeController(IServiceStatusHistoryService serviceStatusHistoryService)
        {
            _serviceStatusHistoryService = serviceStatusHistoryService;
        }

        public IActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("PostSignIn", "Account");
            }

            return View();
        }

        [Route("page-not-found", Name = "PageNotFound")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [Route("no-permission", Name = "FailedLogin")]
        public IActionResult FailedLogin()
        {
            return View();
        }

        public IActionResult Error()
        {
            if (Request.Path.ToString().Contains("404"))
                return RedirectToRoute("PageNotFound");

            if (Request.Path.ToString().Contains("403"))
                return RedirectToRoute("FailedLogin");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Cookies()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = RolesExtensions.AdminUser)]
        [ActionName("Maintenance")]
        [Route("service-under-maintenance", Name = "GetLatestServiceStatusHistory")]
        public async Task<IActionResult> MaintenanceAsync()
        {
            if (!HttpContext.User.HasAdminRole())
            {
                return RedirectToRoute("FailedLogin");
            }
            
            var viewModel = await _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync();

            return View(viewModel);
        }

        [Authorize(Roles = RolesExtensions.AdminUser)]
        [HttpPost]
        [Route("service-under-maintenance", Name = "SaveServiceStatusHistory")]
        public async Task<IActionResult> SaveServiceStatusHistoryAsync(ServiceStatusHistoryViewModel viewModel)
        {
            if (!HttpContext.User.HasAdminRole())
            {
                return RedirectToRoute("FailedLogin");
            }

            await _serviceStatusHistoryService.SaveServiceStatusHistoryAsync(viewModel);

            return RedirectToRoute("GetLatestServiceStatusHistory");
        }

        [Route("service-unavailable", Name = "ServiceUnavailable")]
        public IActionResult ServiceUnavailable()
        {
            return View();
        }
    }
}