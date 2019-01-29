using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class DataImportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDataBlobUploadService _dataBlobUploadService;

        private const string FileMissingKey = "file";
        private const string FileMissingError = "A file must be selected";

        public DataImportController(IMapper mapper, IDataBlobUploadService dataBlobUploadService)
        {
            _mapper = mapper;
            _dataBlobUploadService = dataBlobUploadService;
        }

        public IActionResult Index()
        {
            var viewModel = Populate();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file, int selectedItem)
        {
            if (file == null)
                ModelState.AddModelError(FileMissingKey, FileMissingError);

            if (ModelState.IsValid)
            {
                var viewModel = new SelectedImportDataViewModel { Id = selectedItem };
                _mapper.Map(file, viewModel);
                await _dataBlobUploadService.Upload(viewModel);
            }

            return RedirectToAction(nameof(Index), "DataImport");
        }

        public DataImportParametersViewModel Populate()
        {
            var dataImportTypeNames = Enum.GetNames(typeof(DataImportType));

            var dataImportTypeViewModels = new DataImportParametersViewModel
            {
                ImportType = dataImportTypeNames.Select(uploadType => new SelectListItem
                {

                    Value = uploadType.ToString(),
                    Text = GetDescription(uploadType),
                }).ToArray(),
            };

            return dataImportTypeViewModels;
        }

        //private static int GetId(string uploadType) =>
        //    (int)Enum.Parse(typeof(DataImportType), uploadType);

        private static string GetDescription(string uploadType) =>
            ((DataImportType)Enum.Parse(typeof(DataImportType), uploadType)).Humanize();

    }
}