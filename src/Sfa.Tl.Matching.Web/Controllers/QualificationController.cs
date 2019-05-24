using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class QualificationController : Controller
    {
        [Route("add-qualification/{providerVenueId}", Name = "AddQualification")]
        public IActionResult AddQualification(int providerVenueId)
        {
            return View(new AddQualificationViewModel { ProviderVenueId = providerVenueId });
        }

        [Route("missing-qualification/{providerVenueId}", Name = "MissingQualification")]
        public IActionResult MissingQualification(int providerVenueId)
        {
            return View(new MissingQualificationViewModel { ProviderVenueId = providerVenueId });
        }
    }
}