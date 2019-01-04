using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Infrastucture.Configuration;
using Sfa.Tl.Matching.Web.Models;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MatchingConfiguration _configuration;

        public HomeController(MatchingConfiguration configuration)
        {
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Start()
        {
            //TODO: Begin the journey
            return View("Index");
        }

        [AllowAnonymous]
        public IActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Cookies()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }

        public async Task Signout()
        {
            //TODO: Signout using proper idams methods
            RedirectToAction("Index");

            //Should just do this:
            /*
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(WsFederationDefaults.AuthenticationScheme,
            new AuthenticationProperties
            {
                RedirectUri = "/"
            });
             */
        }

        public async Task SignoutCleanup()
        {
            //TODO: Confirm whether this method is needed
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}