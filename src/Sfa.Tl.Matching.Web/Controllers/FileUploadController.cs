using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize]
    public class FileUploadController : Controller
    {
        private readonly IFileUploadViewModelMapper _viewModelMapper;

        public FileUploadController(IFileUploadViewModelMapper viewModelMapper)
        {
            _viewModelMapper = viewModelMapper;
        }

        public IActionResult Index()
        {
            var viewModel = _viewModelMapper.Populate();

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file, FileUploadViewModel viewModel)
        {
            return View();
        }
    }
}