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
            viewModel.Navigation = LoadCancelLink(opportunityId, opportunityItemId);

            return View("FindEmployer", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityEmployerName(FindEmployerViewModel viewModel)
        {
            var isValidEmployer = await _employerService.ValidateEmployerNameAndId(viewModel.SelectedEmployerId, viewModel.CompanyName);

            if (!isValidEmployer)
            {
                ModelState.AddModelError(nameof(viewModel.CompanyName), "You must find and choose an employer");
                viewModel.Navigation = LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);

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
            viewModel.Navigation = LoadCancelLink(opportunityId, opportunityItemId);

            return View("Details", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Navigation = LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);
                return View("Details", viewModel);
            }

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
            viewModel.Navigation = LoadCancelLink(opportunityId, opportunityItemId);

            return View("CheckDetails", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCheckOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Navigation = LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);
                return View("CheckDetails", viewModel);
            }

            var employerDetailDto = _mapper.Map<EmployerDetailDto>(viewModel);

            await _opportunityService.UpdateOpportunity(employerDetailDto);

            return RedirectToRoute("GetEmployerConsent", new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("saved-opportunities", Name = "GetSavedEmployerOpportunity")]
        public IActionResult SavedEmployerOpportunity()
        {
            return View();
        }

        [HttpGet]
        [Route("remove-employer", Name = "ConfirmDelete")]
        public IActionResult ConfirmDelete()
        {
            return View();
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

            await _opportunityService.ConfirmOpportunities(viewModel.OpportunityId);

            // TODO Send emails
            // await _referralService.SendEmployerReferralEmail(dto.OpportunityId);
            // await _referralService.SendProviderReferralEmail(dto.OpportunityId);

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

        private void Validate(EmployerDetailsViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.EmployerContactPhone))
                return;

            if (!viewModel.EmployerContactPhone.Any(char.IsDigit))
                ModelState.AddModelError(nameof(viewModel.EmployerContactPhone), "You must enter a number");
            else if (!Regex.IsMatch(viewModel.EmployerContactPhone, @"^(?:.*\d.*){7,}$"))
                ModelState.AddModelError(nameof(viewModel.EmployerContactPhone), "You must enter a telephone number that has 7 or more numbers");
        }

        private NavigationViewModel LoadCancelLink(int opportunityId, int opportunityItemId)
        {
            var viewModel = new NavigationViewModel
            {
                CancelText = "Cancel opportunity and start again"
            };

            if (opportunityId == 0) return viewModel;

            var opportunityItemCount = _opportunityService.GetSavedOpportunityItemCountAsync(opportunityId).GetAwaiter().GetResult();
            if (opportunityItemCount == 0)
            {
                viewModel.OpportunityId = opportunityId;
                viewModel.OpportunityItemId = opportunityItemId;
                return viewModel;
            }

            viewModel.CancelText = "Cancel this opportunity";
            viewModel.OpportunityId = opportunityId;
            viewModel.OpportunityItemId = opportunityItemId;

            return viewModel;
        }
    }
}