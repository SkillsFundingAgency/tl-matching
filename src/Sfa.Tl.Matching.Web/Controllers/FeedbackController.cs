using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    //[ServiceFilter(typeof(ServiceUnavailableFilterAttribute))]
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}