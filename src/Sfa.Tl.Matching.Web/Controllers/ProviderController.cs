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
            return View("ProviderSearch", new ProviderSearchParametersViewModel());
        }

        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> Index(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ProviderSearch", viewModel);
            }

            var searchResult = viewModel.UkPrn.HasValue
                ? await _providerService.SearchAsync(viewModel.UkPrn.Value)
                : null;
            
            if (searchResult == null || searchResult.Id == 0)
            {
                return ReturnProviderSearchViewWithInvalidUkPrnError(viewModel);
            }

            return RedirectToRoute("GetProviderDetail", 
                new
                {
                    providerId = searchResult.Id
                });
        }

        private IActionResult ReturnProviderSearchViewWithInvalidUkPrnError(ProviderSearchParametersViewModel viewModel)
        {
            ModelState.AddModelError("UkPrn", "You must enter a real UKPRN");
            return View("ProviderSearch", viewModel);
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public IActionResult ProviderDetail(int providerId)
        {
            return View();
        }

        [HttpGet]
        [Route("hide-provider/{providerId}", Name = "GetConfirmProviderChange")]
        public IActionResult ConfirmProviderChange(int providerId)
        {
            return View();
        }
    }
}