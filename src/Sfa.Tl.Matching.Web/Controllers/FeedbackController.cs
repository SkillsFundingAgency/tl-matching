using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [AllowAnonymous]
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}