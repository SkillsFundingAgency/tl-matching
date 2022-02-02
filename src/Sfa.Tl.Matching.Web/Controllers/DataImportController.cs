using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class DataImportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        public DataImportController(IMapper mapper, IDataBlobUploadService dataBlobUploadService)
        {
            _mapper = mapper;
            _dataBlobUploadService = dataBlobUploadService;
        }

        public IActionResult Index()
        {
            return View(new DataImportParametersViewModel());
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Index(DataImportParametersViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = _mapper.Map<DataUploadDto>(viewModel);
            dto.UserName = HttpContext.User.GetUserName();

            await _dataBlobUploadService.UploadAsync(dto);

            viewModel.IsImportSuccessful = true;

            return View(viewModel);
        }

        private void Validate(DataImportParametersViewModel viewModel)
        {
            if (viewModel.File == null)
            {
                ModelState.AddModelError("file", "You must select a file");
            }

            var fileContentType = viewModel.SelectedImportType.GetFileExtensionType();

            var fileContentTypes = fileContentType?.Split(',');

            if (fileContentTypes is not null && 
                fileContentTypes.All(f => f != viewModel.File?.ContentType))
            {
                ModelState.AddModelError("file", fileContentType.GetFileExtensionErrorMessage(viewModel.File?.ContentType));
            }
        }
    }
}