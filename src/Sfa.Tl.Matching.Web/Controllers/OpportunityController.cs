// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
#endif
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

        [Route("{SearchResultProviderCount}-provisiongap-opportunities-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "SaveProvisionGap")]
        public async Task<IActionResult> SaveProvisionGap(SaveProvisionGapViewModel viewModel)
        {
            var opportunityDto = _mapper.Map<OpportunityDto>(viewModel);
            var opportunityItemDto = _mapper.Map<OpportunityItemDto>(viewModel);

            if (await _opportunityService.IsNewProvisionGapAsync(viewModel.OpportunityItemId))
            {
                var opportunityId = viewModel.OpportunityId;
                if (opportunityId == 0)
                {
                    opportunityId = await CreateOpportunityAsync(opportunityDto);
                }
                opportunityItemDto.OpportunityId = opportunityId;
                return await CreateOpportunityItemAsync(opportunityItemDto);
            }

            var providerSearchDto = new ProviderSearchDto
            {
                OpportunityId = opportunityItemDto.OpportunityId,
                OpportunityItemId = opportunityItemDto.Id,
                SearchRadius = viewModel.SearchRadius,
                Postcode = viewModel.Postcode,
                RouteId = viewModel.SelectedRouteId ?? 0,
                SearchResultProviderCount = viewModel.SearchResultProviderCount
            };
            await _opportunityService.UpdateOpportunityItemAsync(providerSearchDto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id = opportunityItemDto.Id });
        }

        [Route("referral-create", Name = "SaveReferral")]
        public async Task<IActionResult> SaveReferral(string viewModel)
        {
            var saveReferralViewModel = JsonConvert.DeserializeObject<SaveReferralViewModel>(viewModel);

            var opportunityDto = _mapper.Map<OpportunityDto>(saveReferralViewModel);
            var opportunityItemDto = _mapper.Map<OpportunityItemDto>(saveReferralViewModel);

            if (await _opportunityService.IsNewReferralAsync(saveReferralViewModel.OpportunityItemId))
            {
                var opportunityId = saveReferralViewModel.OpportunityId;
                if (opportunityId == 0)
                {
                    opportunityId = await CreateOpportunityAsync(opportunityDto);
                }
                opportunityItemDto.OpportunityId = opportunityId;
                return await CreateOpportunityItemAsync(opportunityItemDto);
            }

            var providerSearchDto = new ProviderSearchDto
            {
                OpportunityId = saveReferralViewModel.OpportunityId,
                OpportunityItemId = saveReferralViewModel.OpportunityItemId,
                SearchRadius = saveReferralViewModel.SearchRadius,
                Postcode = saveReferralViewModel.Postcode,
                RouteId = saveReferralViewModel.SelectedRouteId ?? 0,
                SearchResultProviderCount = saveReferralViewModel.SearchResultProviderCount
            };
            await _opportunityService.UpdateOpportunityItemAsync(providerSearchDto);
            await _opportunityService.UpdateReferrals(opportunityDto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id = saveReferralViewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Get")]
        public async Task<IActionResult> PlacementInformationSave(int id)
        {
            var dto = await _opportunityService.GetPlacementInformationSaveAsync(id);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);

            //TODO: Get these from the back end
            //viewModel.IsReferral = await _opportunityService.IsNewReferral(id);
            //viewModel.CompanyName = "Test Company Name";1

            return View(viewModel);
        }

        [HttpPost]
        [Route("placement-information/{id?}", Name = "PlacementInformationSave_Post")]
        public async Task<IActionResult> PlacementInformationSave(PlacementInformationSaveViewModel viewModel)
        {
            await Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = _mapper.Map<PlacementInformationSaveDto>(viewModel);
            await _opportunityService.UpdateOpportunityItemAsync(dto);

            var isReferralOpportunityItem = await _opportunityService.IsReferralOpportunityItemAsync(viewModel.OpportunityId);
            var opportunityCount = await _opportunityService.GetOpportunityItemCountAsync(viewModel.OpportunityId);

            if (opportunityCount > 1)
            {
                return RedirectToRoute(isReferralOpportunityItem
                    ? "GetCheckAnswersReferrals"
                    : "GetOpportunityBasket",
                    new { id = viewModel.OpportunityId });
            }
            return RedirectToRoute("LoadWhoIsEmployer", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{opportunityItemId}", Name = "GetCheckAnswersReferrals")]
        public async Task<IActionResult> CheckAnswersReferrals(int opportunityItemId)
        {
            var viewModel = await GetCheckAnswersReferralViewModel(opportunityItemId);

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers/{id?}", Name = "SaveCheckAnswersReferrals")]
        public async Task<IActionResult> CheckAnswersReferrals(CheckAnswersReferralViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersReferralViewModel(viewModel.OpportunityId));

            var dto = _mapper.Map<CheckAnswersDto>(viewModel);
            await _opportunityService.UpdateOpportunity(dto);

            await _referralService.SendEmployerReferralEmail(dto.OpportunityId);
            await _referralService.SendProviderReferralEmail(dto.OpportunityId);

            return RedirectToRoute("GetOpportunityBasket", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap/{id?}", Name = "ProvisionGapSent_Get")]
        public async Task<IActionResult> ProvisionGapSent(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);
            var viewModel = _mapper.Map<ProvisionGapSentViewModel>(dto);
            viewModel.EmployerCrmRecord = dto.EmployerCrmId.ToString();

            return View(viewModel);
        }

        [HttpGet]
        [Route("emails-sent/{id?}", Name = "EmailSentReferrals_Get")]
        public async Task<IActionResult> ReferralEmailSent(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);
            var viewModel = _mapper.Map<EmailsSentViewModel>(dto);
            viewModel.EmployerCrmRecord = dto.EmployerCrmId.ToString();

            return View(viewModel);
        }

        [HttpGet]
        [Route("employer-opportunities/{id}", Name = "GetOpportunityBasket")]
        public async Task<IActionResult> OpportunityBasket(int id)
        {
            var viewModel = await _opportunityService.GetOpportunityBasket(id);

            return View(viewModel);
        }

        [HttpPost]
        [Route("add-opportunity", Name = "AddOpportunity")]
        public IActionResult AddOpportunity()
        {
            return RedirectToRoute("Providers_Get");
        }

        [HttpPost]
        [Route("continue-opportunity", Name = "ContinueOpportunity")]
        public async Task<IActionResult> OpportunityBasket(ContinueOpportunityViewModel viewModel)
        {
            return View();
        }

        private async Task<int> CreateOpportunityAsync(OpportunityDto dto)
        {
            var opportunityId = await _opportunityService.CreateOpportunityAsync(dto);
            return opportunityId;
        }

        private async Task<IActionResult> CreateOpportunityItemAsync(OpportunityItemDto dto)
        {
            var opportunityItemId = await _opportunityService.CreateOpportunityItemAsync(dto);

            return RedirectToRoute("PlacementInformationSave_Get", new { id = opportunityItemId });
        }

        private async Task Validate(PlacementInformationSaveViewModel viewModel)
        {
            var opportunityItem = await _opportunityService.GetOpportunityItem(viewModel.OpportunityItemId);
            if (opportunityItem != null)
            {
                viewModel.Postcode = opportunityItem.Postcode;
                viewModel.SearchRadius = opportunityItem.SearchRadius;
                viewModel.RouteId = opportunityItem.RouteId;
            }

            if (viewModel.SearchResultProviderCount > 0 &&
                viewModel.OpportunityType == OpportunityType.ProvisionGap)
            {
                if (!viewModel.NoSuitableStudent &&
                    !viewModel.HadBadExperience &&
                    !viewModel.ProvidersTooFarAway)
                {
                    ModelState.AddModelError(nameof(viewModel.NoSuitableStudent),
                        "You must tell us why the employer did not choose a provider");
                }
            }

            if (!viewModel.PlacementsKnown.HasValue || !viewModel.PlacementsKnown.Value) return;
            if (!viewModel.Placements.HasValue)
                ModelState.AddModelError(nameof(viewModel.Placements), "You must estimate how many students the employer wants for this job at this location");
            else if (viewModel.Placements < 1)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of students must be 1 or more");
            else if (viewModel.Placements > 999)
                ModelState.AddModelError(nameof(viewModel.Placements), "The number of students must be 999 or less");
        }

        private async Task<CheckAnswersReferralViewModel> GetCheckAnswersReferralViewModel(int opportunityItemId)
        {
            var dto = await _opportunityService.GetCheckAnswers(opportunityItemId);
            var providersForReferral = _opportunityService.GetReferrals(opportunityItemId);

            var viewModel = _mapper.Map<CheckAnswersReferralViewModel>(dto);
            viewModel.Providers = _mapper.Map<List<ReferralsViewModel>>(providersForReferral);

            return viewModel;
        }
    }
}