// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
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
                opportunityItemDto.OpportunityId = viewModel.OpportunityId;

                if (opportunityItemDto.OpportunityId == 0)
                {
                    opportunityItemDto.OpportunityId = await CreateOpportunityAsync(opportunityDto);
                }

                var opportunityItemId = await _opportunityService.CreateOpportunityItemAsync(opportunityItemDto);

                return RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
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

            return RedirectToRoute("GetPlacementInformation", new { id = opportunityItemDto.Id });
        }

        [Route("referral-create", Name = "SaveReferral")]
        public async Task<IActionResult> SaveReferral(string viewModel)
        {
            var saveReferralViewModel = JsonConvert.DeserializeObject<SaveReferralViewModel>(viewModel);

            var opportunityDto = _mapper.Map<OpportunityDto>(saveReferralViewModel);
            var opportunityItemDto = _mapper.Map<OpportunityItemDto>(saveReferralViewModel);

            if (await _opportunityService.IsNewReferralAsync(saveReferralViewModel.OpportunityItemId))
            {
                opportunityItemDto.OpportunityId = saveReferralViewModel.OpportunityId;

                if (opportunityItemDto.OpportunityId == 0)
                {
                    opportunityItemDto.OpportunityId = await CreateOpportunityAsync(opportunityDto);
                }

                var opportunityItemId = await _opportunityService.CreateOpportunityItemAsync(opportunityItemDto);

                return RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
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

            return RedirectToRoute("GetPlacementInformation", new { saveReferralViewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("placement-information/{opportunityItemId}", Name = "GetPlacementInformation")]
        public async Task<IActionResult> GetPlacementInformation(int opportunityItemId)
        {
            var dto = await _opportunityService.GetPlacementInformationAsync(opportunityItemId);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);

            return View("PlacementInformation", viewModel);
        }

        [HttpPost]
        [Route("placement-information/{opportunityItemId}", Name = "SavePlacementInformation")]
        public async Task<IActionResult> SavePlacementInformation(PlacementInformationSaveViewModel viewModel)
        {
            await Validate(viewModel);

            if (!ModelState.IsValid)
                return View("PlacementInformation", viewModel);

            var dto = _mapper.Map<PlacementInformationSaveDto>(viewModel);
            await _opportunityService.UpdateOpportunityItemAsync(dto);

            if (viewModel.OpportunityType == OpportunityType.ProvisionGap)
            {
                await _opportunityService.UpdateProvisionGapAsync(dto);
            }

            var isReferralOpportunityItem = await _opportunityService.IsReferralOpportunityItemAsync(viewModel.OpportunityItemId);

            var opportunityCount = await _opportunityService.GetOpportunityItemCountAsync(viewModel.OpportunityId);

            //if First Opp then LoadWhoIsEmployer else if referral then check answer of if provisiongap then OpportunityBasket
            return opportunityCount == 0 ? 
                RedirectToRoute("LoadWhoIsEmployer", new { viewModel.OpportunityId, viewModel.OpportunityItemId }) 
                : isReferralOpportunityItem ? 
                    RedirectToRoute("GetCheckAnswers", new { viewModel.OpportunityItemId }) 
                    : RedirectToRoute("GetOpportunityBasket", new { viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{opportunityItemId}", Name = "GetCheckAnswers")]
        public async Task<IActionResult> GetCheckAnswers(int opportunityItemId)
        {
            var viewModel = await GetCheckAnswersViewModel(opportunityItemId);

            return View("CheckAnswers", viewModel);
        }

        [HttpPost]
        //[Route("check-answers/{opportunityId}-{opportunityItemId}", Name = "SaveCheckAnswers")]
        public async Task<IActionResult> SaveCheckAnswers(int opportunityId, int opportunityItemId)
        {
            if (!ModelState.IsValid)
                return View("CheckAnswers", await GetCheckAnswersViewModel(opportunityItemId));

            await _opportunityService.UpdateOpportunityItemAsync(new CheckAnswersDto
            {
                OpportunityItemId = opportunityItemId, 
                OpportunityId = opportunityId, 
                IsSaved = true
            });

            return RedirectToRoute("GetOpportunityBasket", new { opportunityId });
        }

        [HttpGet]
        [Route("employer-opportunities/{opportunityId}", Name = "GetOpportunityBasket")]
        public async Task<IActionResult> OpportunityBasket(int opportunityId)
        {
            var viewModel = await _opportunityService.GetOpportunityBasket(opportunityId);

            return View(viewModel);
        }

        // TODO FIX reuse this method later
        [HttpPost]
        [Route("send-emails/{opportunityItemId}", Name = "SendEmails")]
        public async Task<IActionResult> SendEmails(CheckAnswersViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //    return View(await GetCheckAnswersViewModel(viewModel.OpportunityItemId));

            var dto = _mapper.Map<CheckAnswersDto>(viewModel);
            await _opportunityService.UpdateOpportunityItemAsync(dto);

            await _referralService.SendEmployerReferralEmail(dto.OpportunityId);
            await _referralService.SendProviderReferralEmail(dto.OpportunityId);

            return RedirectToRoute("GetOpportunityBasket", new { id = dto.OpportunityId });
        }

        [HttpGet]
        [Route("emails-sent/{id}", Name = "EmailSentReferrals_Get")]
        public async Task<IActionResult> ReferralEmailSent(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);
            var viewModel = _mapper.Map<SentViewModel>(dto);
            viewModel.EmployerCrmRecord = dto.EmployerCrmId.ToString();

            return View(viewModel);
        }

        [HttpGet]
        [Route("remove-opportunityItem/{opportunityId}/{opportunityItemId}", Name = "RemoveAndGetOpportunityBasket")]
        public async Task<IActionResult> RemoveOpportunityItemAndGetOpportunityBasket(int opportunityId, int opportunityItemId)
        {
            await _opportunityService.RemoveOpportunityItemAsync(opportunityId, opportunityItemId);
            var opportunityItemCount = await _opportunityService.GetOpportunityItemCountAsync(opportunityId);

            return opportunityItemCount == 0
                ? RedirectToRoute("Start")
                : RedirectToRoute("GetOpportunityBasket", new {id = opportunityId});
        }

        [HttpPost]
        [Route("continue-opportunity", Name = "SaveSelectedOpportunities")]
        public async Task<IActionResult> SaveSelectedOpportunities(ContinueOpportunityViewModel viewModel)
        {
            if (viewModel.SubmitAction == "Finish")
                return RedirectToRoute("Start");

            if (viewModel.SelectedOpportunity.Any(p => p.IsSelected))
                return View("EmployerConsent");

            ModelState.AddModelError("Model.ReferralItems[0].IsSelected", "You must select an opportunity to continue");

            var opportunityBasketViewModel = await _opportunityService.GetOpportunityBasket(viewModel.OpportunityId);

            return View(nameof(OpportunityBasket), opportunityBasketViewModel);
        }

        private async Task<int> CreateOpportunityAsync(OpportunityDto dto)
        {
            var opportunityId = await _opportunityService.CreateOpportunityAsync(dto);
            return opportunityId;
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

        private async Task<CheckAnswersViewModel> GetCheckAnswersViewModel(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetCheckAnswers(opportunityItemId);
            //var providersForReferral = _opportunityService.GetReferrals(opportunityItemId);

            //var viewModel = _mapper.Map<CheckAnswersViewModel>(dto);
            //viewModel.Providers = _mapper.Map<List<ReferralsViewModel>>(providersForReferral);

            return viewModel;
        }
    }
}