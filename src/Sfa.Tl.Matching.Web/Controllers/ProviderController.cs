﻿using System.Threading.Tasks;
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
                    ukPrn = searchResult.UkPrn
                });
        }

        private IActionResult ReturnProviderSearchViewWithInvalidUkPrnError(ProviderSearchParametersViewModel viewModel)
        {
            ModelState.AddModelError("UkPrn", "You must enter a real UKPRN");
            return View("ProviderSearch", viewModel);
        }

        [HttpGet]
        [Route("provider-overview/{ukPrn}", Name = "GetProviderDetail")]
        public async Task<IActionResult> ProviderDetail(long ukPrn)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
            {
                viewModel = await _providerService.GetByIdAsync(providerId);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-overview/{providerId}", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (!ProviderDetailIsValid(viewModel))
            {
                return View("ProviderDetail", viewModel);
            }

            if (viewModel.Id > 0)
            {
                await _providerService.UpdateProvider(viewModel);
            }
            else
            {
                viewModel.Id = await _providerService.CreateProvider(viewModel);
            }

            return View("ProviderDetail", viewModel);
        }

        private bool ProviderDetailIsValid(ProviderDetailViewModel viewModel)
        {
            var result = ModelState.IsValid;

            if (string.IsNullOrWhiteSpace(viewModel.PrimaryContactEmail))
            {
                result = false;
            }

            return result;
        }

        [HttpGet]
        [Route("hide-unhide/{ukPrn}", Name = "GetConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(long ukPrn)
        {
            var provider = await _providerService.GetProviderByUkPrnAsync(ukPrn);
            return View(new HideProviderViewModel
            {
                ProviderId = provider.Id,
                UkPrn = ukPrn,
                ProviderName = provider.Name,
                IsActive = provider.IsEnabledForSearch
            });
        }

        [HttpPost]
        [Route("hide-unhide/{ukPrn}", Name = "ConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(HideProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderChange", viewModel);
            }

            await _providerService.SetIsProviderEnabledAsync(viewModel.ProviderId, !viewModel.IsActive);

            return RedirectToRoute("GetProviderDetail",
                new
                {
                    ukPrn = viewModel.UkPrn
                });
        }
    }
}