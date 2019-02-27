using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.Dto;
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
        public IActionResult FindEmployer()
        {
            var opportunityId = (int)TempData["OpportunityId"];

            var viewModel = new FindEmployerViewModel
            {
                OpportunityId = opportunityId
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("who-is-employer", Name = "EmployerName_Post")]
        public async Task<IActionResult> FindEmployer(FindEmployerViewModel viewModel)
        {
            TempData["OpportunityId"] = viewModel.OpportunityId;

            if (viewModel.SelectedEmployerId == 0)
            {
                ModelState.AddModelError(nameof(viewModel.BusinessName), "You must find and choose an employer");
                return View(viewModel);
            }

            var employer = await _employerService.GetEmployer(viewModel.SelectedEmployerId);
            if (employer == null)
                ModelState.AddModelError(nameof(viewModel.BusinessName), "You must find and choose an employer");

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await PopulateDto(viewModel.OpportunityId, employer);
            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("EmployerDetails_Get");
        }

        [HttpGet]
        [Route("employer-search", Name = "EmployerSearch_Get")]
        public IActionResult Search(string query)
        {
            var employers = _employerService.Search(query);

            return Ok(employers.ToList());
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
                EmployerName = dto.EmployerName,
                Contact = dto.Contact,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone
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

        private async Task<OpportunityDto> PopulateDto(int opportunityId, EmployerDto employer)
        {
            var dto = await _opportunityService.GetOpportunity(opportunityId);
            dto.EmployerCrmId = employer.CrmId;
            dto.EmployerName = employer.CompanyName; // TODO AU Should this also inclue the Aka?
            dto.EmployerAupa = employer.Aupa;
            dto.EmployerOwner = employer.Owner;
            dto.Contact = employer.PrimaryContact;
            dto.ContactEmail = employer.Email;
            dto.ContactPhone = employer.Phone;
            dto.ModifiedBy = HttpContext.User.GetUserName();

            return dto;
        }
    }
}