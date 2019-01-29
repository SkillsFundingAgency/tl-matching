using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.Services;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Controllers
{
    //[Authorize]
    public class DataImportController : Controller
    {
        private readonly IDataImportViewModelMapper _viewModelMapper;
        private readonly IUploadService _uploadService;

        private const string FileMissingKey = "file";
        private const string FileMissingError = "A file must be selected";

        public DataImportController(IDataImportViewModelMapper viewModelMapper,
            IUploadService uploadService)
        {
            _viewModelMapper = viewModelMapper;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var viewModel = _viewModelMapper.Populate();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, DataImportViewModel viewModel)
        {
            if (file == null)
                ModelState.AddModelError(FileMissingKey, FileMissingError);

            if (!ModelState.IsValid)
            {
                viewModel = _viewModelMapper.Populate();
                return View("Index", viewModel);
            }

            await _uploadService.Upload(file, viewModel);

            viewModel = _viewModelMapper.Populate();

            return View("Index", viewModel);
        }
    }
}