using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
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
        private readonly IMapper _mapper;

        public EmployerController(IEmployerService employerService, IOpportunityService opportunityService, IMapper mapper)
        {
            _employerService = employerService;
            _opportunityService = opportunityService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("employer-search", Name = "EmployerSearch_Get")]
        public IActionResult Search(string query)
        {
            var employers = _employerService.Search(query);

            return Ok(employers.ToList());
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
            var employerDto = viewModel.SelectedEmployerId != 0 ?
                await _employerService.GetEmployer(viewModel.SelectedEmployerId) :
                null;

            if (!ModelState.IsValid || employerDto == null)
            {
                if (employerDto == null)
                {
                    ModelState.AddModelError(nameof(viewModel.CompanyName), "You must find and choose an employer");
                }

                return View(viewModel);
            }

            var dto = _mapper.Map<EmployerNameDto>(viewModel);

            await _opportunityService.SaveEmployerName(dto);

            return RedirectToRoute("EmployerDetails_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("employer-details/{id?}", Name = "EmployerDetails_Get")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);

            var employerDto = await _employerService.GetEmployer(dto.EmployerId ?? 0);

            var viewModel = _mapper.Map(employerDto, new EmployerDetailsViewModel { OpportunityId = dto.Id });

            return View(viewModel);
        }

        [HttpPost]
        [Route("employer-details/{id?}", Name = "EmployerDetails_Post")]
        public async Task<IActionResult> Details(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.SaveEmployerDetail(employerDetailDto);

            var dto = await _opportunityService.GetOpportunityWithReferrals(viewModel.OpportunityId);

            return RedirectToRoute(dto.IsReferral.HasValue && dto.IsReferral.Value ?
                "CheckAnswersReferrals_Get" :
                "CheckAnswersProvisionGap_Get");
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