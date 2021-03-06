﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderVenueController : Controller
    {
        private readonly IProviderVenueService _providerVenueService;

        public ProviderVenueController(IProviderVenueService providerVenueService)
        {
            _providerVenueService = providerVenueService;
        }

        [Route("add-venue/{providerId}", Name = "AddProviderVenue")]
        public IActionResult AddProviderVenue(int providerId)
        {
            return View("AddProviderVenue", new AddProviderVenueViewModel { ProviderId = providerId });
        }

        [HttpPost]
        [Route("add-venue/{providerId}", Name = "CreateVenue")]
        public async Task<IActionResult> CreateVenueAsync(AddProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddProviderVenue", viewModel);

            var (isValid, formattedPostcode) = await _providerVenueService.IsValidPostcodeAsync(viewModel.Postcode);
            viewModel.Postcode = formattedPostcode;

            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                return View("AddProviderVenue", viewModel);
            }

            var venue = await _providerVenueService.GetVenueAsync(viewModel.ProviderId, viewModel.Postcode);

            int venueId;
            if (venue != null)
                venueId = venue.Id;
            else
                venueId = await _providerVenueService.CreateVenueAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = venueId });
        }
        
        [Route("venue-overview/{providerVenueId}", Name = "GetProviderVenueDetail")]
        public async Task<IActionResult> GetProviderVenueDetailAsync(int providerVenueId, int providerId = 0)
        {
            var isFromAddVenue = providerId == 0;
            var viewModel = await Populate(providerVenueId);
            viewModel.IsFromAddVenue = isFromAddVenue;

            return View("ProviderVenueDetail",viewModel);
        }

        [HttpPost]
        [Route("venue-overview/{providerVenueId}", Name = "SaveProviderVenueDetail")]
        public async Task<IActionResult> SaveProviderVenueDetailAsync(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(viewModel.Id);
                return View("ProviderVenueDetail", viewModel);
            }

            await _providerVenueService.UpdateVenueAsync(viewModel);

            var isSaveSection = IsSaveSection(viewModel.SubmitAction);
            if (isSaveSection)
            {
                return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = viewModel.Id, providerId = viewModel.ProviderId });
            }

            if (viewModel.Qualifications == null || viewModel.Qualifications.Count == 0)
            {
                return RedirectToRoute("AddQualification", new { providerVenueId = viewModel.Id });
            }

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        [HttpGet]
        [ActionName("GetConfirmRemoveProviderVenue")]
        [Route("remove-venue/{providerVenueId}", Name = "GetConfirmRemoveProviderVenue")]
        public async Task<IActionResult> GetConfirmRemoveProviderVenueAsync(int providerVenueId)
        {
            var viewModel = await _providerVenueService.GetRemoveProviderVenueViewModelAsync(providerVenueId);

            return View("ConfirmRemoveProviderVenue", viewModel);
        }

        [HttpPost]
        [Route("remove-venue/{providerVenueId}", Name = "ConfirmRemoveProviderVenue")]
        public async Task<IActionResult> ConfirmRemoveProviderVenueAsync(RemoveProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmRemoveProviderVenue", viewModel);
            }

            await _providerVenueService.UpdateVenueAsync(viewModel);

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        private async Task<ProviderVenueDetailViewModel> Populate(int providerVenueId)
        {
            var viewModel = await _providerVenueService.GetVenueWithQualificationsAsync(providerVenueId);

            return viewModel ?? new ProviderVenueDetailViewModel();
        }

        private static bool IsSaveSection(string submitAction)
        {
            return !string.IsNullOrWhiteSpace(submitAction) && string.Equals(submitAction,
                       "SaveSection", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}