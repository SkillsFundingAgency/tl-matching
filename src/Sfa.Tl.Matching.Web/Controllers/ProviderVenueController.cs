using System;
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

        [Route("add-venue/{providerId}", Name = "AddVenue")]
        public IActionResult AddProviderVenue(int providerId)
        {
            return View(new AddProviderVenueViewModel { ProviderId = providerId });
        }

        [HttpPost]
        [Route("add-venue/{providerId}", Name = "CreateVenue")]
        public async Task<IActionResult> AddProviderVenue(AddProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var (isValid, formatedPostCode) = await _providerVenueService.IsValidPostCodeAsync(viewModel.Postcode);
            viewModel.Postcode = formatedPostCode;

            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                return View(viewModel);
            }

            var venue = await _providerVenueService.GetVenue(viewModel.ProviderId, viewModel.Postcode);

            int venueId;
            if (venue != null)
                venueId = venue.Id;
            else
                venueId = await _providerVenueService.CreateVenueAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = venueId });
        }
        
        [Route("venue-overview/{providerVenueId}", Name = "GetProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(int providerVenueId, int providerId = 0)
        {
            var isFromAddVenue = providerId == 0;
            var viewModel = await Populate(providerVenueId);
            viewModel.IsFromAddVenue = isFromAddVenue;

            return View(viewModel);
        }

        [HttpPost]
        [Route("venue-overview/{providerVenueId}", Name = "SaveProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(viewModel.Id);
                return View("ProviderVenueDetail", viewModel);
            }

            await _providerVenueService.UpdateVenueAsync(viewModel);

            var isSaveSection = IsSaveSection(viewModel.SubmitAction);
            if (isSaveSection)
                return View("ProviderVenueDetail", viewModel);

            //if (viewModel.Qualifications == null || viewModel.Qualifications.Count == 0)
            //{
            //    ModelState.AddModelError("Qualifications", "You must add a qualification for this venue");
            //    viewModel = await Populate(viewModel.Id);
            //    return View(viewModel);
            //}

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        [HttpGet]
        [Route("hide-unhide-venue/{providerVenueId}/{providerId?}", Name = "GetConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(int providerVenueId, int providerId = 0)
        {
            var viewModel = await _providerVenueService.GetHideProviderVenueViewModelAsync(providerVenueId);

            viewModel.ProviderId = providerId;

            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide-venue/{providerVenueId}", Name = "ConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(HideProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderVenueChange", viewModel);
            }

            viewModel.IsRemoved = !viewModel.IsRemoved;
            await _providerVenueService.UpdateVenueAsync(viewModel);
            
            return  viewModel.ProviderId == 0 
                ? RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = viewModel.ProviderVenueId })
                : RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
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