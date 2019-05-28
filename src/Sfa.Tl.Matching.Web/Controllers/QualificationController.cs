using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class QualificationController : Controller
    {
        private readonly IProviderVenueService _providerVenueService;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;

        public QualificationController(
            IProviderVenueService providerVenueService, 
            IQualificationService qualificationService,
            IProviderQualificationService providerQualificationService)
        {
            _providerVenueService = providerVenueService;
            _qualificationService = qualificationService;
            _providerQualificationService = providerQualificationService;
        }
        
        [Route("add-qualification/{providerVenueId}", Name = "AddQualification")]
        public async Task<IActionResult> AddQualification(int providerVenueId)
        {
            var postcode = await _providerVenueService.GetVenuePostcodeAsync(providerVenueId);

            return View(new AddQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                Postcode = postcode
            });
        }

        [HttpPost]
        [Route("add-qualification/{providerId}", Name = "CreateQualification")]
        public async Task<IActionResult> AddQualification(AddQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var isValid = await _qualificationService.IsValidLarIdAsync(viewModel.LarsId);

            if (string.IsNullOrWhiteSpace(viewModel.LarsId) || !isValid)
            {
                ModelState.AddModelError("LarsId", "Enter a learning aim reference (LAR) that has 8 characters");
                return View(viewModel);
            }

            var qualification = await _qualificationService.GetQualificationAsync(viewModel.LarsId);

            if (qualification == null)
            {
                return RedirectToRoute("MissingQualification", new { providerVenueId = viewModel.ProviderVenueId });
            }

            viewModel.QualificationId = qualification.Id;
            await _providerQualificationService.CreateProviderQualificationAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId = viewModel.ProviderVenueId });
        }

        [Route("missing-qualification/{providerVenueId}", Name = "MissingQualification")]
        public IActionResult MissingQualification(int providerVenueId)
        {
            return View(new MissingQualificationViewModel { ProviderVenueId = providerVenueId })
                ;
        }

        [HttpPost]
        [Route("missing-qualification/{providerVenueId}", Name = "MissingQualification")]
        public IActionResult MissingQualification(MissingQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            //var qualificationId = qualification?.Id 
            //                      ?? await _qualificationService.CreateQualificationAsync(viewModel);

            return View(viewModel);
        }
    }
}