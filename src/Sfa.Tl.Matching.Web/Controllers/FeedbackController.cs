// ReSharper disable RedundantUsingDirective
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
#endif
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}