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
                    opportunityItemDto.OpportunityId = await CreateOpportunityAsync(opportunityDto);

                opportunityItemDto.ProvisionGap = new List<ProvisionGapDto>
                {
                    new ProvisionGapDto()
                };

                var opportunityItemId = await _opportunityService.CreateOpportunityItemAsync(opportunityItemDto);

                return RedirectToRoute("GetPlacementInformation", new { opportunityItemId });
            }

            var providerSearchDto = new ProviderSearchDto
            {
                OpportunityId = opportunityItemDto.OpportunityId,
                OpportunityItemId = opportunityItemDto.OpportunityItemId,
                SearchRadius = viewModel.SearchRadius,
                Postcode = viewModel.Postcode,
                RouteId = viewModel.SelectedRouteId ?? 0,
                SearchResultProviderCount = viewModel.SearchResultProviderCount
            };
            await _opportunityService.UpdateOpportunityItemAsync(providerSearchDto);

            return RedirectToRoute("GetPlacementInformation", new { opportunityItemDto.OpportunityItemId });
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
            await _opportunityService.UpdateReferrals(opportunityItemDto);

            return RedirectToRoute("GetPlacementInformation", new { saveReferralViewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("placement-information/{opportunityItemId}", Name = "GetPlacementInformation")]
        public async Task<IActionResult> GetPlacementInformation(int opportunityItemId)
        {
            var dto = await _opportunityService.GetPlacementInformationAsync(opportunityItemId);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);
            viewModel.Navigation = LoadCancelLink(viewModel.OpportunityId, opportunityItemId);

            return View("PlacementInformation", viewModel);
        }

        [HttpPost]
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

            var opportunityItemCount = await _opportunityService.GetOpportunityItemCountAsync(viewModel.OpportunityId);

            //if First Opp (saved opportunity items == 0) then LoadWhoIsEmployer else if referral then check answer of if provisiongap then OpportunityBasket
            return opportunityItemCount == 0 ?
                RedirectToRoute("LoadWhoIsEmployer", new { viewModel.OpportunityId, viewModel.OpportunityItemId })
                : viewModel.OpportunityType == OpportunityType.Referral ?
                    await GetCheckAnswers(viewModel.OpportunityItemId)
                    : await SaveCheckAnswers(viewModel.OpportunityId, viewModel.OpportunityItemId);
        }

        [HttpGet]
        [Route("check-answers/{opportunityItemId}", Name = "GetCheckAnswers")]
        public async Task<IActionResult> GetCheckAnswers(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetCheckAnswers(opportunityItemId);
            viewModel.Navigation = LoadCancelLink(viewModel.OpportunityId, opportunityItemId);
            return View("CheckAnswers", viewModel);
        }

        //[HttpPost] //Removed because there is a redirect from SaveOpportunityEmployerDetails
        public async Task<IActionResult> SaveCheckAnswers(int opportunityId, int opportunityItemId)
        {
            await _opportunityService.UpdateOpportunityItemAsync(new CheckAnswersDto
            {
                OpportunityItemId = opportunityItemId,
                OpportunityId = opportunityId,
                IsSaved = true
            });

            return RedirectToRoute("GetOpportunityBasket", new { opportunityId, opportunityItemId });
        }

        [HttpGet]
        [Route("employer-opportunities/{opportunityId}-{opportunityItemId}", Name = "GetOpportunityBasket")]
        public async Task<IActionResult> OpportunityBasket(int opportunityId, int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetOpportunityBasket(opportunityId);
            viewModel.OpportunityItemId = opportunityItemId;
            return View(viewModel);
        }

        // TODO FIX reuse this method later
        [HttpPost]
        public async Task<IActionResult> SendEmails(CheckAnswersViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //    return View(await GetCheckAnswersViewModel(viewModel.OpportunityItemId));

            var dto = _mapper.Map<CheckAnswersDto>(viewModel);
            await _opportunityService.UpdateOpportunityItemAsync(dto);

            await _referralService.SendEmployerReferralEmail(dto.OpportunityId);
            await _referralService.SendProviderReferralEmail(dto.OpportunityId);

            return RedirectToRoute("GetOpportunityBasket", new { dto.OpportunityId, dto.OpportunityItemId });
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

        [HttpPost]
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

        private NavigationViewModel LoadCancelLink(int opportunityId, int opportunityItemId)
        {
            var viewModel = new NavigationViewModel
            {
                CancelText = "Cancel opportunity and start again",
                CancelRouteName = "Start"
            };

            if (opportunityId == 0) return viewModel;

            var opportunityItemCount = _opportunityService.GetOpportunityItemCountAsync(opportunityId).GetAwaiter().GetResult();
            if (opportunityItemCount == 0)
                return viewModel;

            viewModel.CancelRouteName = "RemoveAndGetOpportunityBasket";
            viewModel.CancelText = "Cancel this opportunity";
            viewModel.OpportunityId = opportunityId;
            viewModel.OpportunityItemId = opportunityItemId;

            return viewModel;
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

        [Route("remove-opportunity", Name = "ConfirmDelete")]
        public IActionResult ConfirmDelete()
        {
            return View();
        }
    }
}