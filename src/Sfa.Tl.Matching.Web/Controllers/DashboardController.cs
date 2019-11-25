using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IEmployerService _employerService;

        public DashboardController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [Route("Start", Name = "Start")]
        public async Task<IActionResult> Start()
        {
            var username = HttpContext.User.GetUserName();
            var savedOpportunitiesCount = await _employerService.GetInProgressEmployerOpportunityCountAsync(username);

            return View(new DashboardViewModel
            {
                HasSavedOppportunities = savedOpportunitiesCount > 0
            });
        }
    }
}