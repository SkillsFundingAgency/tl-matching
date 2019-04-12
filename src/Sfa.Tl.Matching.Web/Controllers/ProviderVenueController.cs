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

        [Route("venue-overview-{postcode}", Name = "SearchVenue")]
        public async Task<IActionResult> ProviderVenueDetail(string postcode)
        {
            var viewModel = await Populate(postcode);

            return View(viewModel);
        }

        private async Task<ProviderVenueDetailViewModel> Populate(string postcode)
        {
            var viewModel = await _providerVenueService.GetVenueWithQualifications(postcode);

            return viewModel;
        }

        [HttpPost]
        [Route("venue-overview-{postcode}", Name = "UpdateVenue")]
        public async Task<IActionResult> SaveVenue(string postcode, ProviderVenueDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await Populate(postcode);
                return View("ProviderVenueDetail", viewModel);
            }

            await _providerVenueService.UpdateVenue(viewModel);
            viewModel = await Populate(postcode);

            return View("ProviderVenueDetail", viewModel);
        }

        [HttpPost]
        [Route("venue-overvieww-{postcode}", Name = "SaveAcademicYears")]
        public IActionResult SaveAcademicYears(string postcode, ProviderVenueDetailViewModel viewModel)
        {
            // TODO Update Academic Years
            
            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
        }

        [Route("add-venue/{providerId}", Name = "AddVenue")]
        public IActionResult ProviderVenueAdd(int providerId)
        {
            return View(new ProviderVenueAddViewModel
            {
                ProviderId = providerId
            });
        }

        [HttpPost]
        [Route("add-venue/{providerId}", Name = "CreateVenue")]
        public async Task<IActionResult> ProviderVenueAdd(ProviderVenueAddViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var (isValid, formatedPostCode) = await _providerVenueService.IsValidPostCode(viewModel.Postcode);
            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                return View(viewModel);
            }

            viewModel.Postcode = formatedPostCode;

            var dto = _mapper.Map<ProviderVenueDto>(viewModel);

            await _providerVenueService.CreateVenue(dto);

            return RedirectToRoute("SearchVenue", new { postcode = viewModel.Postcode });
        }
    }
}