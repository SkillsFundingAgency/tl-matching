// ReSharper disable RedundantUsingDirective

using System;
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
        public async Task<IActionResult> GetOpportunityCompanyName(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerAsync(opportunityId, opportunityItemId);
            viewModel.Navigation = await _opportunityService.LoadCancelLink(opportunityId, opportunityItemId);

            return View("FindEmployer", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityCompanyName(FindEmployerViewModel viewModel)
        {
            await ValidateAsync(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Navigation = await _opportunityService.LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);
                return View("FindEmployer", viewModel);
            }

            var dto = _mapper.Map<CompanyNameDto>(viewModel);

            await _opportunityService.UpdateOpportunity(dto);

            return RedirectToRoute("GetEmployerDetails", new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("employer-details/{opportunityId}-{opportunityItemId}", Name = "GetEmployerDetails")]
        public async Task<IActionResult> GetOpportunityEmployerDetails(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _employerService.GetOpportunityEmployerDetailAsync(opportunityId, opportunityItemId);
            viewModel.Navigation = await _opportunityService.LoadCancelLink(opportunityId, opportunityItemId);

            return View("Details", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Navigation = await _opportunityService.LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);
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
            viewModel.Navigation = await _opportunityService.LoadCancelLink(opportunityId, opportunityItemId);

            return View("CheckDetails", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveCheckOpportunityEmployerDetails(EmployerDetailsViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Navigation = await _opportunityService.LoadCancelLink(viewModel.OpportunityId, viewModel.OpportunityItemId);
                return View("CheckDetails", viewModel);
            }

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
        public async  Task<IActionResult> ConfirmDelete(int opportunityId)
        {
            var dto = await _employerService.GetConfirmDeleteEmployerOpportunity(opportunityId,
                HttpContext.User.GetUserName());

            var viewModel = new RemoveEmployerViewModel
            {
                OpportunityId = opportunityId,
                Count = dto.OpportunityCount,
                ConfirmDeleteText = dto.OpportunityCount == 1
                    ? $"Confirm you want to delete {dto.OpportunityCount} opportunity created for {dto.EmployerName}?"
                    : $"Confirm you want to delete {dto.OpportunityCount} opportunities created for {dto.EmployerName}?",
                WarningDeleteText = dto.EmployerCount == 1
                    ? "This cannot be undone and will mean you have no more employers with saved opportunities."
                    : "This cannot be undone."
            };
            
            return View(viewModel);
        }

        [HttpGet]
        [Route("remove-employer/{opportunityId}", Name = "DeleteEmployer")]
        public async Task<IActionResult> DeleteEmployer(int opportunityId)
        {
            var employerOpportunities =
                await _employerService.GetSavedEmployerOpportunitiesAsync(HttpContext.User.GetUserName());

            //TODO: Delete employer goes here...

            var employerCount =  employerOpportunities.EmployerOpportunities.Count;

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

        private async Task ValidateAsync(FindEmployerViewModel viewModel)
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

                if (!string.IsNullOrEmpty(lockedByUser) 
                    && lockedByUser != HttpContext.User.GetUserName())
                {
                    ModelState.AddModelError(nameof(viewModel.CompanyName),
                        $"{lockedByUser} is already working on this employer’s opportunities. " +
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