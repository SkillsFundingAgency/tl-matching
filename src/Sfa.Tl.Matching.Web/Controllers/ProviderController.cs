using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet]
        [Route("search-ukprn", Name = "GetProviderSearch")]
        public IActionResult Index()
        {
            return View("SearchProvider", new ProviderSearchParametersViewModel());
        }

        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> Index(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("SearchProvider", viewModel);
            }

            var searchResult = viewModel.UkPrn.HasValue
                ? await _providerService.SearchAsync(viewModel.UkPrn.Value)
                : null;

            if (searchResult == null || searchResult.Id == 0)
            {
                ModelState.AddModelError("UkPrn", "You must enter a real UKPRN");
                return View("SearchProvider", viewModel);
            }

            return RedirectToRoute("GetProviderDetail", new { providerId = searchResult.Id });
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public async Task<IActionResult> ProviderDetail(int providerId)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
            {
                viewModel = await _providerService.GetProviderDetailByIdAsync(providerId, true);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-overview", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ProviderDetail", viewModel);
            }

            if (viewModel.Id > 0)
            {
                await _providerService.UpdateProviderDetail(viewModel);
            }
            else
            {
                viewModel.Id = await _providerService.CreateProvider(viewModel);
            }

            return View("ProviderDetail", viewModel);
        }

        [HttpGet]
        [Route("hide-unhide-provider/{providerId}", Name = "GetConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(int providerId)
        {
            var viewModel = await _providerService.GetHideProviderViewModelAsync(providerId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide-provider/{providerId}", Name = "ConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(HideProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderChange", viewModel);
            }

            await _providerService.SetIsProviderEnabledForSearchAsync(viewModel.ProviderId, !viewModel.IsEnabledForSearch);

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }
    }
}