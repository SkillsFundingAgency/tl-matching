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
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    [ServiceFilter(typeof(BackLinkFilter))]
    [ServiceFilter(typeof(ServiceUnavailableFilterAttribute))]
    public class EmployerController : Controller
    {
        private readonly IEmployerService _employerService;
        private readonly IOpportunityService _opportunityService;
        private readonly IReferralService _referralService;
        private readonly IMapper _mapper;

        public EmployerController(IEmployerService employerService,
                                    IOpportunityService opportunityService,
                                    IReferralService referralService,
                                    IMapper mapper)
        {
            _employerService = employerService;
            _opportunityService = opportunityService;
            _referralService = referralService;
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
        public async Task<IActionResult> GetOpportunityCompanyName(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerAsync(opportunityId, opportunityItemId);

            return View("FindEmployer", viewModel);
        }

        [HttpPost]
        [Route("who-is-employer/{opportunityId}-{opportunityItemId}")]
        public async Task<IActionResult> SaveOpportunityCompanyName(FindEmployerViewModel viewModel)
        {
            var username = HttpContext.User.GetUserName();

            await ValidateAsync(viewModel, username);

            if (!ModelState.IsValid)
                return View("FindEmployer", viewModel);

            var dto = _mapper.Map<CompanyNameDto>(viewModel);

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
        [Route("employer-details/{opportunityId}-{opportunityItemId}")]
        public async Task<IActionResult> SaveOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View("Details", viewModel);

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.UpdateOpportunity(employerDetailDto);

            var isReferralOpportunityItem = await _opportunityService.IsReferralOpportunityItemAsync(viewModel.OpportunityItemId);

            if (isReferralOpportunityItem)
                return RedirectToRoute("GetCheckAnswers", new { viewModel.OpportunityItemId });

            return RedirectToAction("SaveCheckAnswers", "Opportunity",
               new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("check-employer-details/{opportunityId}-{opportunityItemId}", Name = "CheckEmployerDetails")]
        public async Task<IActionResult> GetCheckOpportunityEmployerDetails(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerDetailAsync(opportunityId, opportunityItemId);

            return View("CheckDetails", viewModel);
        }

        [HttpPost]
        [Route("check-employer-details/{opportunityId}-{opportunityItemId}")]
        public async Task<IActionResult> SaveCheckOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View("CheckDetails", viewModel);

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.UpdateOpportunity(employerDetailDto);

            return RedirectToRoute("GetEmployerConsent", new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("saved-opportunities", Name = "GetSavedEmployerOpportunity")]
        public async Task<IActionResult> SavedEmployerOpportunity()
        {
            var username = HttpContext.User.GetUserName();
            var viewModel = await _employerService.GetSavedEmployerOpportunitiesAsync(username);

            return View(viewModel);
        }

        [HttpGet]
        [Route("confirm-remove-employer/{opportunityId}", Name = "ConfirmDelete")]
        public async Task<IActionResult> ConfirmDelete(int opportunityId)
        {
            var dto = await _employerService.GetConfirmDeleteEmployerOpportunity(opportunityId,
                HttpContext.User.GetUserName());

            var viewModel = new RemoveEmployerViewModel
            {
                OpportunityId = opportunityId,
                OpportunityCount = dto.OpportunityCount,
                EmployerName = dto.EmployerName,
                EmployerCount = dto.EmployerCount
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("remove-employer/{opportunityId}", Name = "DeleteEmployer")]
        public async Task<IActionResult> DeleteEmployer(int opportunityId)
        {

            await _opportunityService.DeleteEmployerOpportunityItemAsync(opportunityId);

            var employerOpportunities =
                await _employerService.GetSavedEmployerOpportunitiesAsync(HttpContext.User.GetUserName());

            var employerCount = employerOpportunities.EmployerOpportunities.Count;

            return employerCount == 0
                ? RedirectToRoute("Start")
                : RedirectToRoute("GetSavedEmployerOpportunity");
        }

        [HttpGet]
        [Route("permission/{opportunityId}-{opportunityItemId}", Name = "GetEmployerConsent")]
        public async Task<IActionResult> EmployerConsent(int opportunityId, int opportunityItemId)
        {
            var viewModel = await GetEmployerConsentViewModel(opportunityId, opportunityItemId);

            return View(viewModel);
        }

        [HttpPost]
        [Route("permission/{opportunityId}-{opportunityItemId}", Name = "SaveEmployerConsent")]
        public async Task<IActionResult> EmployerConsent(EmployerConsentViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetEmployerConsentViewModel(viewModel.OpportunityId, viewModel.OpportunityItemId));

            await _referralService.ConfirmOpportunities(viewModel.OpportunityId, HttpContext.User.GetUserName());

            return RedirectToRoute("EmailSentReferrals_Get", new { id = viewModel.OpportunityId });
        }

        private async Task<EmployerConsentViewModel> GetEmployerConsentViewModel(int opportunityId, int opportunityItemId)
        {
            var viewModel = new EmployerConsentViewModel
            {
                OpportunityId = opportunityId,
                OpportunityItemId = opportunityItemId,
                Details = await _employerService.GetOpportunityEmployerDetailAsync(opportunityId, opportunityItemId),
                OpportunityItemCount =
                    await _opportunityService.GetReferredOpportunityItemCountAsync(opportunityId)
            };

            return viewModel;
        }

        private async Task ValidateAsync(FindEmployerViewModel viewModel, string currentUser)
        {
            var isValidEmployer =
                await _employerService.ValidateCompanyNameAndId(viewModel.SelectedEmployerId, viewModel.CompanyName);

            if (!isValidEmployer)
            {
                ModelState.AddModelError(nameof(viewModel.CompanyName), "You must find and choose an employer");
            }
            else
            {
                var lockedByUser = await _employerService
                    .GetEmployerOpportunityOwnerAsync(viewModel.SelectedEmployerId);

                if (!string.IsNullOrEmpty(lockedByUser))
                {
                    if (lockedByUser == currentUser)
                        ModelState.AddModelError(nameof(viewModel.CompanyName),
                            "You are already working on this employer’s opportunities. Please start again and find this employer in your saved opportunities.");
                    else
                        ModelState.AddModelError(nameof(viewModel.CompanyName),
                            "Your colleague, " + $"{lockedByUser}, is already working on this employer’s opportunities. " +
                            "Please choose a different employer.");
                }
            }
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