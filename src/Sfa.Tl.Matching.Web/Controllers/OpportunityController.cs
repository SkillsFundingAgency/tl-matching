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
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    [ServiceFilter(typeof(BackLinkFilter))]
    [ServiceFilter(typeof(ServiceUnavailableFilterAttribute))]
    public class OpportunityController : Controller
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IMapper _mapper;

        public OpportunityController(IOpportunityService opportunityService, IMapper mapper)
        {
            _opportunityService = opportunityService;
            _mapper = mapper;
        }

        [Route("{SearchResultProviderCount}-provisiongap-opportunities-within-one-hour-of-{Postcode}-for-route-{SelectedRouteId}", Name = "SaveProvisionGap")]
        public async Task<IActionResult> SaveProvisionGapAsync(SaveProvisionGapViewModel viewModel)
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
        public async Task<IActionResult> SaveReferralAsync()
        {
            var selectedProviders = TempData["SelectedProviders"] as string;
            var saveReferralViewModel = JsonConvert.DeserializeObject<SaveReferralViewModel>(selectedProviders);

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
            await _opportunityService.UpdateReferralsAsync(opportunityItemDto);

            return RedirectToRoute("GetPlacementInformation", new { saveReferralViewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("placement-information/{opportunityItemId}", Name = "GetPlacementInformation")]
        public async Task<IActionResult> GetPlacementInformationAsync(int opportunityItemId)
        {
            var dto = await _opportunityService.GetPlacementInformationAsync(opportunityItemId);

            var viewModel = _mapper.Map<PlacementInformationSaveViewModel>(dto);

            return View("PlacementInformation", viewModel);
        }

        [HttpPost]
        [Route("placement-information/{opportunityItemId}", Name = "SavePlacementInformation")]
        public async Task<IActionResult> SavePlacementInformationAsync(PlacementInformationSaveViewModel viewModel)
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

            var opportunityItemCount = await _opportunityService.GetSavedOpportunityItemCountAsync(viewModel.OpportunityId);

            //if First Opp (saved opportunity items == 0) then LoadWhoIsEmployer else if referral then check answer of if provisiongap then OpportunityBasket
            return opportunityItemCount == 0 ?
                RedirectToRoute("GetOpportunityCompanyName", new { viewModel.OpportunityId, viewModel.OpportunityItemId })
                : viewModel.OpportunityType == OpportunityType.Referral ?
                    RedirectToRoute("GetCheckAnswers", new { viewModel.OpportunityItemId })
                    : await SaveCheckAnswers(viewModel.OpportunityId, viewModel.OpportunityItemId);
        }

        [HttpGet]
        [Route("check-answers/{opportunityItemId}", Name = "GetCheckAnswers")]
        public async Task<IActionResult> GetCheckAnswersAsync(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetCheckAnswersAsync(opportunityItemId);

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
        public async Task<IActionResult> OpportunityBasketAsync(int opportunityId, int opportunityItemId)
        {
            await _opportunityService.ClearOpportunityItemsSelectedForReferralAsync(opportunityId);

            var viewModel = await _opportunityService.GetOpportunityBasketAsync(opportunityId);

            viewModel.OpportunityItemId = opportunityItemId != 0 ? opportunityItemId
                : viewModel.OpportunityItemId =
                    (viewModel.ReferralItems.LastOrDefault()?.OpportunityItemId != 0
                        ? viewModel.ReferralItems.LastOrDefault()?.OpportunityItemId :
                        viewModel.ProvisionGapItems.LastOrDefault()?.OpportunityItemId) ?? 0;

            return View("OpportunityBasket", viewModel);
        }

        [HttpGet]
        [Route("emails-sent/{opportunityId}", Name = "EmailSentReferrals_Get")]
        public async Task<IActionResult> ReferralEmailSentAsync(int opportunityId)
        {
            var dto = await _opportunityService.GetOpportunityAsync(opportunityId);
            var viewModel = _mapper.Map<SentViewModel>(dto);
            viewModel.EmployerCrmRecord = dto.EmployerCrmId.ToString();

            return View("ReferralEmailSent", viewModel);
        }

        [HttpPost]
        [Route("employer-opportunities/{opportunityId}-{opportunityItemId}", Name = "SaveSelectedOpportunities")]
        public async Task<IActionResult> SaveSelectedOpportunitiesAsync(ContinueOpportunityViewModel viewModel)
        {
            if (viewModel.SubmitAction == "SaveSelectedOpportunities")
            {
                Validate(viewModel);
                if (!ModelState.IsValid)
                {
                    var opportunityBasketViewModel = await _opportunityService.GetOpportunityBasketAsync(viewModel.OpportunityId);

                    return View("OpportunityBasket", opportunityBasketViewModel);
                }
            }

            await _opportunityService.ContinueWithOpportunitiesAsync(viewModel);

            return viewModel.SubmitAction == "CompleteProvisionGaps" ?
                RedirectToRoute("Start") :
                RedirectToRoute("GetEmployerConsent", new { viewModel.OpportunityId, viewModel.OpportunityItemId });
        }

        [HttpGet]
        [Route("remove-opportunity/{opportunityItemId}", Name = "GetConfirmDeleteOpportunityItem")]
        public async Task<IActionResult> ConfirmDeleteOpportunityItemAsync(int opportunityItemId)
        {
            var viewModel = await _opportunityService.GetConfirmDeleteOpportunityItemAsync(opportunityItemId);

            return View("ConfirmDeleteOpportunityItem", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOpportunityItemAsync(DeleteOpportunityItemViewModel viewModel)
        {
            await _opportunityService.DeleteOpportunityItemAsync(viewModel.OpportunityId, viewModel.OpportunityItemId);

            return viewModel.BasketItemCount == 1 ?
                RedirectToRoute("Start") :
                RedirectToRoute("GetOpportunityBasket", new { viewModel.OpportunityId, OpportunityItemId = 0 });
        }

        [HttpGet]
        [Route("download-opportunity/{opportunityId}", Name = "DownloadOpportunitySpreadsheet")]
        public async Task<IActionResult> DownloadOpportunitySpreadsheetAsync(int opportunityId)
        {
            var downloadedFileInfo = await _opportunityService.GetOpportunitySpreadsheetDataAsync(opportunityId);

            return File(downloadedFileInfo.FileContent, 
                downloadedFileInfo.ContentType,
                downloadedFileInfo.FileName);
        }

        [HttpGet]
        [Route("remove-referral/{referralId}-{opportunityItemId}", Name = "DeleteReferral")]
        public async Task<IActionResult> DeleteReferralAsync(int referralId, int opportunityItemId)
        {
            await _opportunityService.DeleteReferralAsync(referralId);
            return RedirectToRoute("GetCheckAnswers", new { opportunityItemId });
        }

        private async Task<int> CreateOpportunityAsync(OpportunityDto dto)
        {
            var opportunityId = await _opportunityService.CreateOpportunityAsync(dto);
            return opportunityId;
        }

        private async Task Validate(PlacementInformationSaveViewModel viewModel)
        {
            var opportunityItem = await _opportunityService.GetOpportunityItemAsync(viewModel.OpportunityItemId);
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

        private void Validate(ContinueOpportunityViewModel viewModel)
        {
            if (!viewModel.IsReferralSelected)
                ModelState.AddModelError("ReferralItems[0].IsSelected", "You must select an opportunity to continue");
        }
    }
}