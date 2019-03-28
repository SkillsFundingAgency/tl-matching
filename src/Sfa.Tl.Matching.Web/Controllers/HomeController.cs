using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
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

        [Route("system-error", Name = "SystemError")]
        public IActionResult SystemError()
        {
            return View();
        }

        public IActionResult Error()
        {
            var requestPath = Request.Path.ToString();

            if (requestPath.Contains(((int)HttpStatusCode.Forbidden).ToString()))
                return RedirectToRoute(nameof(FailedLogin));

            if (requestPath.Contains(((int)HttpStatusCode.NotFound).ToString()))
                return RedirectToRoute(nameof(PageNotFound));

            if (IsErrorStatusCode(requestPath))
                return RedirectToRoute(nameof(SystemError));

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

        private static bool IsErrorStatusCode(string path)
        {
            if (path.Contains(((int)HttpStatusCode.InternalServerError).ToString()))
                return true;

            if (path.Contains(((int)HttpStatusCode.BadGateway).ToString()))
                return true;

            if (path.Contains(((int)HttpStatusCode.ServiceUnavailable).ToString()))
                return true;

            if (path.Contains(((int)HttpStatusCode.GatewayTimeout).ToString()))
                return true;

            return false;
        }
    }
}