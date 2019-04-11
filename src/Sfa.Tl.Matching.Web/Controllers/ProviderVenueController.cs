using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderVenueController : Controller
    {
        [Route("overview-{postcode}", Name = "SearchVenue")]
        public IActionResult ProviderVenueDetail(string postcode)
        {
            var viewModel = new ProviderVenueDetailViewModel();

            return View(viewModel);
        }
    }
}