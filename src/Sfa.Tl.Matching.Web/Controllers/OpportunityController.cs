using System.Collections.Generic;
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
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class OpportunityController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IReferralService _referralService;
		private readonly IMapper _mapper;

        public OpportunityController(IOpportunityService opportunityService, IReferralService referralService, IMapper mapper)
        {
            _opportunityService = opportunityService;
            _referralService = referralService;
            _mapper = mapper;
        }

        [Route("{SearchResultProviderCount}-provisiongap-opportunities-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "OpportunityCreate_Get")]
        public async Task<IActionResult> CreateProvisionGap(CreateProvisionGapViewModel viewModel)
        {
            var dto = _mapper.Map<OpportunityDto>(viewModel);

            var id = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferral(CreateReferralViewModel viewModel)
        {
            var dto = _mapper.Map<OpportunityDto>(viewModel);

            var id = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Get")]
        public async Task<IActionResult> PlacementInformationSave(int id)
        {
            var dto = await _opportunityService.GetPlacementInformationSave(id);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Post")]
        public async Task<IActionResult> PlacementInformationSave(PlacementInformationSaveViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = _mapper.Map<PlacementInformationSaveDto>(viewModel);
            await _opportunityService.Save(dto);

            return RedirectToRoute("EmployerFind_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "CheckAnswersReferrals_Get")]
        public async Task<IActionResult> CheckAnswersReferrals(int id)
        {
            var viewModel = await GetCheckAnswersReferralViewModel(id);

            return View(viewModel);
        }
        
        [HttpPost]
        [Route("check-answers/{id?}", Name = "CheckAnswersReferrals_Post")]
        public async Task<IActionResult> CheckAnswersReferrals(CheckAnswersReferralViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersReferralViewModel(viewModel.OpportunityId));

            var dto = _mapper.Map<CheckAnswersDto>(viewModel);
            await _opportunityService.Save(dto);

            return RedirectToRoute("EmailSentReferrals_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersProvisionGap_Get")]
        public async Task<IActionResult> CheckAnswersProvisionGap(int id)
        {
            var dto = await _opportunityService.GetCheckAnswers(id);

            var viewModel = _mapper.Map<CheckAnswersProvisionGapViewModel>(dto);

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersProvisionGap_Post")]
        public async Task<IActionResult> CheckAnswersProvisionGap(CheckAnswersProvisionGapViewModel viewModel)
        {
            var dto = _mapper.Map<CheckAnswersDto>(viewModel);

            await _opportunityService.Save(dto);
            await _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToRoute("ProvisionGapSent_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap/{id?}", Name = "ProvisionGapSent_Get")]
        public async Task<IActionResult> ProvisionGapSent(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            return View(new ProvisionGapSentViewModel
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

            await _referralService.SendProviderReferralEmail(id);

            return View(new EmailsSentViewModel
            {
                EmployerContactName = opportunity.EmployerContact,
                EmployerBusinessName = opportunity.EmployerName
            });
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

        private async Task<CheckAnswersReferralViewModel> GetCheckAnswersReferralViewModel(int id)
        {
            var dto = await _opportunityService.GetCheckAnswers(id);
            var providersForReferral = _opportunityService.GetReferrals(id);

            var viewModel = _mapper.Map<CheckAnswersReferralViewModel>(dto);
            viewModel.Providers = _mapper.Map<List<ReferralsViewModel>>(providersForReferral);

            return viewModel;
        }
    }
}