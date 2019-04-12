using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderController : Controller
    {
        public IActionResult ProviderDetail()
        {
            return View();
        }
    }
}