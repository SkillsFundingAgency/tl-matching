using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderVenueController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderVenueService _providerVenueService;

        public ProviderVenueController(IMapper mapper,
            IProviderVenueService providerVenueService)
        {
            _mapper = mapper;
            _providerVenueService = providerVenueService;
        }

        [Route("add-venue/{providerId}", Name = "AddVenue")]
        public IActionResult AddProviderVenue(int providerId)
        {
            return View(new AddProviderVenueViewModel
            {
                ProviderId = providerId
            });
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
            {
                venueId = venue.Id;
            }
            else
            {
                var dto = _mapper.Map<ProviderVenueDto>(viewModel);
                venueId = await _providerVenueService.CreateVenueAsync(dto);
            }

            return RedirectToRoute("GetProviderVenueDetail", new
            {
                id = venueId
            });
        }
        
        [Route("venue-overview/{id}", Name = "GetProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(int id)
        {
            var viewModel = await Populate(id);

            return View(viewModel);
        }

        [HttpPost]
        [Route("venue-overview/{id}", Name = "UpdateVenue")]
        public async Task<IActionResult> SaveVenue(ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(viewModel.Id);
                return View("ProviderVenueDetail", viewModel);
            }

            var dto = _mapper.Map<UpdateProviderVenueDto>(viewModel);
            await _providerVenueService.UpdateVenueAsync(dto);
            viewModel = await Populate(viewModel.Id);

            return View("ProviderVenueDetail", viewModel);
        }

        [HttpPost]
        [Route("venue-overview", Name = "SaveProviderVenueDetail")]
        public async Task<IActionResult> ProviderVenueDetail(ProviderVenueDetailViewModel viewModel)
        {
            // TODO Update Academic Years
            if (viewModel.Qualifications == null || viewModel.Qualifications.Count == 0)
            {
                ModelState.AddModelError("Qualifications", "You must add a qualification for this venue");
                viewModel = await Populate(viewModel.Id);
                return View(viewModel);
            }

            return RedirectToRoute("GetProviderDetail", new { id = viewModel.ProviderId });
        }

        [HttpGet]
        [Route("hide-unhide/{id}", Name = "GetConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(int id)
        {
            var viewModel = await Populate(id);
            //TODO: Move view model creation to repository
            var viewModel = new HideProviderVenueViewModel
            {
                ProviderVenueId = providerVenueViewModel.Id,
                UkPrn = ukPrn,
                Postcode = providerVenueViewModel.Postcode,
                ProviderName = providerVenueViewModel.ProviderName,
                IsEnabledForSearch = providerVenueViewModel.IsEnabledForSearch
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide/{ukPrn}/{postcode}", Name = "ConfirmProviderVenueChange")]
        public async Task<IActionResult> ConfirmProviderVenueChange(HideProviderVenueViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderVenueChange", viewModel);
            }

            await _providerVenueService.SetIsProviderVenueEnabledForSearchAsync(viewModel.ProviderVenueId, !viewModel.IsEnabledForSearch);

            return RedirectToRoute("GetProviderVenueDetail",
                new
                {
                    id = viewModel.Id
                });
        }

        private async Task<ProviderVenueDetailViewModel> Populate(int id)
        {
            var viewModel = await _providerVenueService.GetVenueWithQualificationsAsync(id);

            return viewModel ?? new ProviderVenueDetailViewModel();
        }
    }
}