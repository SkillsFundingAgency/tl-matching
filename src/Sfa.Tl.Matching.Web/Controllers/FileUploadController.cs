using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index() =>
            View();

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            return View();
        }
    }
}