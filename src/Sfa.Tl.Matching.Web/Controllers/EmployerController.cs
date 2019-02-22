using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

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
        [Route("who-is-employer", Name = "EmployerName_Get")]
        public IActionResult Name()
        {
            var opportunityId = (int)TempData["OpportunityId"];

            var viewModel = new EmployerNameViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("who-is-employer", Name = "EmployerName_Post")]
        public IActionResult Name(EmployerNameViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

            if (!ModelState.IsValid)
                return View(viewModel);

            return RedirectToRoute("EmployerDetails_Get");
        }

        [HttpGet]
        [Route("employer-search", Name = "EmployerSearch_Get")]
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
        [Route("employer-details", Name = "EmployerDetails_Get")]
        public IActionResult Details()
        {
            var opportunityId = (int)TempData["OpportunityId"];

            var viewModel = new EmployerDetailsViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("employer-details", Name = "EmployerDetails_Post")]
        public IActionResult Details(EmployerDetailsViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

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