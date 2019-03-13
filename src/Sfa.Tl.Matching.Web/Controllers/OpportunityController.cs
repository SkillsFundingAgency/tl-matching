using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class OpportunityController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IReferralService _referralService;

        public OpportunityController(IOpportunityService opportunityService, IReferralService referralService)
        {
            _opportunityService = opportunityService;
            _referralService = referralService;
        }

        [Route("{searchResultProviderCount}-opportunities-within-{distance}-miles-of-{postcode}-for-route-{routeId}", Name = "OpportunityCreate_Get")]
        public async Task<IActionResult> CreateProvisionGap(int searchResultProviderCount, int routeId, string postcode, short distance)
        {
            var dto = new OpportunityDto
            {
                RouteId = routeId,
                Postcode = postcode,
                Distance = distance,
                SearchResultProviderCount = searchResultProviderCount,
                CreatedBy = HttpContext.User.GetUserName(),
                UserEmail = HttpContext.User.GetUserEmail()
            };

            var id = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("PlacementInformationSave_Get", new
            {
                id
            });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Get")]
        public async Task<IActionResult> PlacementInformationSave(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);

            var viewModel = new PlacementInformationSaveViewModel
            {
                RouteId = dto.RouteId,
                Postcode = dto.Postcode,
                Distance = dto.Distance,
                OpportunityId = dto.Id,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown,
                Placements = !dto.PlacementsKnown.HasValue || !dto.PlacementsKnown.Value ? 
                    default : dto.Placements
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Post")]
        public async Task<IActionResult> PlacementInformationSave(PlacementInformationSaveViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.SavePlacementInformation(viewModel);

            return RedirectToRoute("EmployerFind_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "CheckAnswersReferrals_Get")]
        public async Task<IActionResult> CheckAnswersReferrals(int id)
        {
            var viewModel = await GetCheckAnswersViewModel(id);

            return View(viewModel);
        }
        
        [HttpPost]
        [Route("check-answers/{id?}", Name = "CheckAnswersReferrals_Post")]
        public async Task<IActionResult> CheckAnswersReferrals(CheckAnswersReferralViewModel referralViewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersViewModel(referralViewModel.OpportunityId));

            referralViewModel.CreatedBy = HttpContext.User.GetUserName();

            //await _opportunityService.CreateReferral(referralViewModel);

            return RedirectToRoute("EmailSentReferrals_Get", new { id = referralViewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersProvisionGap_Get")]
        public async Task<IActionResult> CheckAnswersProvisionGap(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersProvisionGapViewModel
            {
                OpportunityId = dto.Id,
                PlacementInformation = GetPlacementViewModel(dto)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersProvisionGap_Post")]
        public async Task<IActionResult> CheckAnswersProvisionGap(CheckAnswersProvisionGapViewModel viewModel)
        {
            viewModel.CreatedBy = HttpContext.User.GetUserName();

            await _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToRoute("EmailSentProvisionGap_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap/{id?}", Name = "EmailSentProvisionGap_Get")]
        public async Task<IActionResult> EmailSentProvisionGap(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            await _referralService.SendProvisionGapEmail(id);

            return View(new EmailSentProvisionGapViewModel
            {
                EmployerContactName = opportunity.EmployerContact,
                Postcode = opportunity.Postcode,
                RouteName = opportunity.RouteName
            });
        }

        [HttpGet]
        [Route("emails-sent/{id?}", Name = "EmailSentReferrals_Get")]
        public async Task<IActionResult> EmailSentReferrals(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            return View(new EmailsSentViewModel
            {
                EmployerContactName = opportunity.EmployerContact,
                EmployerBusinessName = opportunity.EmployerName
            });
        }

        private static CheckAnswersPlacementViewModel GetPlacementViewModel(OpportunityDto dto)
        {
            var viewModel = new CheckAnswersPlacementViewModel
            {
                Contact = dto.EmployerContact,
                Distance = dto.Distance,
                EmployerName = dto.EmployerName,
                JobTitle = dto.JobTitle,
                PlacementsKnown = dto.PlacementsKnown,
                Placements = dto.Placements,
                Postcode = dto.Postcode,
                RouteName = dto.RouteName
            };

            return viewModel;
        }
        private async Task<CheckAnswersReferralViewModel> GetCheckAnswersViewModel(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersReferralViewModel
            {
                OpportunityId = dto.Id,
                PlacementInformation = GetPlacementViewModel(dto),
                Providers = _opportunityService.GetReferrals(id),
            };

            return viewModel;
        }

        private void Validate(PlacementInformationSaveViewModel viewModel)
        {
            if (!viewModel.PlacementsKnown.HasValue || !viewModel.PlacementsKnown.Value) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many placements the employer wants at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of placements must be 999 or less");
        }
    }
}