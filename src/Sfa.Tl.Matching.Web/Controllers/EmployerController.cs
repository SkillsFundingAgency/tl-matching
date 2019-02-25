using System.Linq;
using System.Threading.Tasks;
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
        private readonly IOpportunityService _opportunityService;

        public EmployerController(IEmployerService employerService,
            IOpportunityService opportunityService)
        {
            _employerService = employerService;
            _opportunityService = opportunityService;
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
        public async Task<IActionResult> Name(EmployerNameViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

            await Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.EmployerName = viewModel.BusinessName;
            dto.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("EmployerDetails_Get");
        }

        [HttpGet]
        [Route("employer-search", Name = "EmployerSearch_Get")]
        public async Task<IActionResult> Search(string query)
        {
            var employers = await _employerService.Search(query);
            var employersSelectList = employers.Select(e => e.EmployerNameWithAka).ToList();

            return Ok(employersSelectList);
        }

        private async Task Validate(EmployerNameViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.BusinessName)) return;

            var employer = await _employerService.GetEmployer(viewModel.CompanyName, viewModel.AlsoKnownAs);
            if (employer == null)
                ModelState.AddModelError(nameof(viewModel.BusinessName), "You must find and choose an employer");
        }

        [HttpGet]
        [Route("employer-details", Name = "EmployerDetails_Get")]
        public async Task<IActionResult> Details()
        {
            var opportunityId = (int)TempData["OpportunityId"];
            var dto = await _opportunityService.GetOpportunity(opportunityId);

            var viewModel = new EmployerDetailsViewModel
            {
                OpportunityId = opportunityId,
                EmployerName = dto.EmployerName
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("employer-details", Name = "EmployerDetails_Post")]
        public async Task<IActionResult> Details(EmployerDetailsViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.Contact = viewModel.Contact;
            dto.ContactEmail = viewModel.ContactEmail;
            dto.ContactPhone = viewModel.ContactPhone;
            dto.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToAction(nameof(OpportunityController.CheckAnswers), "Opportunity");
        }

        [HttpPost]
        public IActionResult GoBack(EmployerDetailsViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;
            
            return RedirectToRoute("EmployerName_Get");
        }
    }
}