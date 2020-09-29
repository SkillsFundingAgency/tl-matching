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
        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public DashboardController(IEmployerService employerService, IServiceStatusHistoryService serviceStatusHistoryService)
        {
            _employerService = employerService;
            _serviceStatusHistoryService = serviceStatusHistoryService;
        }

        [Route("Start", Name = "Start")]
        public async Task<IActionResult> Start()
        {
            var username = HttpContext.User.GetUserName();
            var savedOpportunitiesCount = await _employerService.GetInProgressEmployerOpportunityCountAsync(username);
            var serviceStatusHistory = await _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync();

            return View(new DashboardViewModel
            {
                HasSavedOpportunities = savedOpportunitiesCount > 0,
                IsServiceOnline = serviceStatusHistory.IsOnline
            });
        }
    }
}