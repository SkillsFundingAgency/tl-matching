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
        [Route("who-is-employer/{opportunityId}-{opportunityItemId}", Name = "LoadWhoIsEmployer")]
        public async Task<IActionResult> GetOpportunityEmployerName(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerAsync(opportunityId, opportunityItemId);

            return View("FindEmployer", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityEmployerName(FindEmployerViewModel viewModel)
        {
            var isValidEmployer = await _employerService.ValidateEmployerNameAndId(viewModel.SelectedEmployerId, viewModel.CompanyName);

            if (!isValidEmployer)
            {
                ModelState.AddModelError(nameof(viewModel.CompanyName), "You must find and choose an employer");
                return View("FindEmployer", viewModel);
            }

            var dto = _mapper.Map<EmployerNameDto>(viewModel);

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("GetEmployerDetails", new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("employer-details/{opportunityId}-{opportunityItemId}", Name = "GetEmployerDetails")]
        public async Task<IActionResult> GetOpportunityEmployerDetails(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerDetailAsync(opportunityId, opportunityItemId);

            return View("Details", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View("Details", viewModel);

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.UpdateOpportunity(employerDetailDto);

            var isReferralOpportunityItem = await _opportunityService.IsReferralOpportunityItemAsync(viewModel.OpportunityItemId);

            if (isReferralOpportunityItem)
                return RedirectToRoute("GetCheckAnswers", new {viewModel.OpportunityItemId});
           
            return RedirectToAction("SaveCheckAnswers", "Opportunity",
               new {viewModel.OpportunityId, viewModel.OpportunityItemId});
        }

        [HttpGet]
        [Route("saved-opportunities", Name = "GetSavedEmployerOpportunity")]
        public IActionResult SavedEmployerOpportunity()
        {
            return View();
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