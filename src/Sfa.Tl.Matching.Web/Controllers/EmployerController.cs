using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
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
        [Route("who-is-employer/{id?}", Name = "EmployerFind_Get")]
        public IActionResult FindEmployer(int id)
        {
            var viewModel = new FindEmployerViewModel
            {
                OpportunityId = id
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("who-is-employer/{id?}", Name = "EmployerFind_Post")]
        public async Task<IActionResult> FindEmployer(FindEmployerViewModel viewModel)
        {
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

            var dto = PopulateDto(viewModel.OpportunityId, employer);
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
        [Route("employer-details/{id?}", Name = "EmployerDetails_Get")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);

            var viewModel = new EmployerDetailsViewModel
            {
                OpportunityId = dto.Id,
                EmployerName = dto.EmployerName,
                Contact = dto.EmployerContact,
                ContactEmail = dto.EmployerContactEmail,
                ContactPhone = dto.EmployerContactPhone
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("employer-details/{id?}", Name = "EmployerDetails_Post")]
        public async Task<IActionResult> Details(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = await _opportunityService.GetOpportunity(viewModel.OpportunityId);

            dto.EmployerContact = viewModel.Contact;
            dto.EmployerContactEmail = viewModel.ContactEmail;
            dto.EmployerContactPhone = viewModel.ContactPhone;
            dto.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute(dto.IsReferral ?
                "CheckAnswers_Get" :
                "CheckAnswersGap_Get");
        }

        private OpportunityDto PopulateDto(int opportunityId, EmployerDto employer)
        {
            var dto = new OpportunityDto
            {
                Id = opportunityId,
                EmployerName = employer.CompanyName, // TODO AU Should this also inclue the Aka?
                EmployerContact = employer.PrimaryContact,
                EmployerContactEmail = employer.Email,
                EmployerContactPhone = employer.Phone,
                ModifiedBy = HttpContext.User.GetUserName()
            };

            return dto;
        }

        private void Validate(EmployerDetailsViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ContactPhone))
                return;

            if (!viewModel.ContactPhone.Any(char.IsDigit))
                ModelState.AddModelError(nameof(viewModel.ContactPhone), "You must enter a number");
            else if (!Regex.IsMatch(viewModel.ContactPhone, @"^(?:.*\d.*){7,}$"))
                ModelState.AddModelError(nameof(viewModel.ContactPhone), "You must enter a telephone number that has 7 or more numbers");
        }
    }
}