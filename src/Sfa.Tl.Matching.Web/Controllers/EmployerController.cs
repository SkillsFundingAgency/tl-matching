// ReSharper disable RedundantUsingDirective
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
#if !NoAuth
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
#endif
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
        [Route("employer-search", Name = "SearchEmployer")]
        public IActionResult Search(string query)
        {
            var employers = _employerService.Search(query);

            return Ok(employers.ToList());
        }

        [HttpGet]
        [Route("who-is-employer/{id}", Name = "LoadWhoIsEmployer")]
        public async Task<IActionResult> GetOpportunityEmployerName(int id)
        {
            var viewModel = await _opportunityService.GetOpportunityEmployerAsync(id);

            return View("FindEmployer", viewModel);
        }

        [HttpPost]
        [Route("who-is-employer/{id}", Name = "SaveEmployerName")]
        public async Task<IActionResult> SaveOpportunityEmployerName(FindEmployerViewModel viewModel)
        {
            var employerDto = viewModel.SelectedEmployerId != 0 &&
                !string.IsNullOrEmpty(viewModel.CompanyName) ?
                await _employerService.GetEmployer(viewModel.SelectedEmployerId) :
                null;

            if (employerDto == null || viewModel.CompanyName != employerDto.CompanyNameWithAka)
            {
                ModelState.AddModelError(nameof(viewModel.CompanyName), "You must find and choose an employer");
                return View("FindEmployer", viewModel);
            }

            var dto = _mapper.Map<EmployerNameDto>(viewModel);
            dto.CompanyName = employerDto.CompanyNameWithAka;

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("GetEmployerDetails", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("employer-details/{id?}", Name = "GetEmployerDetails")]
        public async Task<IActionResult> GetOpportunityEmployerDetails(int id)
        {
            var viewModel = await _employerService.GetOpportunityEmployerDetails(id);

            return View("Details", viewModel);
        }

        [HttpPost]
        [Route("employer-details/{id?}", Name = "SaveEmployerDetails")]
        public async Task<IActionResult> SaveOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View("Details", viewModel);

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.UpdateOpportunity(employerDetailDto);

            var isReferralOpportunityItem = await _opportunityService.IsReferralOpportunityItemAsync(viewModel.OpportunityId);

            return RedirectToRoute(isReferralOpportunityItem ? "GetCheckAnswers" : "GetOpportunityBasket", new { id = viewModel.OpportunityId });
        }

        private void Validate(EmployerDetailsViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.EmployerContactPhone))
                return;

            if (!viewModel.EmployerContactPhone.Any(char.IsDigit))
                ModelState.AddModelError(nameof(viewModel.EmployerContactPhone), "You must enter a number");
            else if (!Regex.IsMatch(viewModel.EmployerContactPhone, @"^(?:.*\d.*){7,}$"))
                ModelState.AddModelError(nameof(viewModel.EmployerContactPhone), "You must enter a telephone number that has 7 or more numbers");
        }
    }
}