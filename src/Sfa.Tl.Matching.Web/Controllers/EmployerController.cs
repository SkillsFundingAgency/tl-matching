using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Constants;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class EmployerController : Controller
    {
        private readonly IEmployerService _employerService;

        public EmployerController(IEmployerService employerService)
        {
            _employerService = employerService;
        }

        [HttpGet]
        [Route(RouteTemplates.EmployerName, Name = RouteNames.EmployerNameGet)]
        public IActionResult Name()
        {
            var opportunityId = (int)TempData[TempDataKeys.OpportunityId];

            var viewModel = new EmployerNameViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route(RouteTemplates.EmployerName, Name = RouteNames.EmployerNamePost)]
        public IActionResult Name(EmployerNameViewModel viewModel)
        {
            TempData[TempDataKeys.OpportunityId] = viewModel.OpportunityId;

            if (!ModelState.IsValid)
                return View(viewModel);

            return RedirectToRoute(RouteNames.EmployerDetailsGet);
        }

        [HttpGet]
        [Route(RouteTemplates.EmployerSearch, Name = RouteNames.EmployerSearchGet)]
        public IActionResult Search(string query)
        {
            var employers = new List<string>();
            employers.Add("Employer1");
            employers.Add("Employer2");
            employers.Add("Employer3");
            employers.Add("Employer4");

            return Ok(employers);
        }

        [HttpGet]
        [Route(RouteTemplates.EmployerDetails, Name = RouteNames.EmployerDetailsGet)]
        public IActionResult Details()
        {
            var opportunityId = (int)TempData[TempDataKeys.OpportunityId];

            var viewModel = new EmployerDetailsViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route(RouteTemplates.EmployerDetails, Name = RouteNames.EmployerDetailsPost)]
        public IActionResult Details(EmployerDetailsViewModel viewModel)
        {
            TempData[TempDataKeys.OpportunityId] = viewModel.OpportunityId;

            if (!ModelState.IsValid)
                return View(viewModel);

            return View();
        }

        public IActionResult Check()
        {
            return View();
        }
    }
}