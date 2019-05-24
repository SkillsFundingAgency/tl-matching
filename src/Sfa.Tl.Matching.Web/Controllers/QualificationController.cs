using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class QualificationController : Controller
    {
        private readonly IProviderVenueService _providerVenueService;
        private readonly IQualificationService _qualificationService;

        public QualificationController(
            IProviderVenueService providerVenueService, 
            IQualificationService qualificationService)
        {
            _providerVenueService = providerVenueService;
            _qualificationService = qualificationService;
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

        [Route("missing-qualification/{providerVenueId}", Name = "MissingQualification")]
        public IActionResult MissingQualification(int providerVenueId)
        {
            return View(new MissingQualificationViewModel { ProviderVenueId = providerVenueId });
        }
    }
}