using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Infrastructure.Extensions;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class OpportunityController : Controller
    {
        public OpportunityController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}