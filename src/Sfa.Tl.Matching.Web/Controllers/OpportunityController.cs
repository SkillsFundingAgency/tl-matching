﻿using System.Threading.Tasks;
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

        public OpportunityController(IOpportunityService opportunityService)
        {
            _opportunityService = opportunityService;
        }

        [Route("opportunity-within-{distance}-miles-of-{postcode}-for-route-{routeId}", Name = "OpportunityCreate_Get")]
        public async Task<IActionResult> CreateProvisionGap(int routeId, string postcode, short distance)
        {
            var dto = new OpportunityDto
            {
                RouteId = routeId,
                Postcode = postcode,
                Distance = distance,
                Providers = 0,
                CreatedBy = HttpContext.User.GetUserName(),
                UserEmail = HttpContext.User.GetUserEmail()
            };

            var id = await _opportunityService.CreateOpportunity(dto);

            return RedirectToRoute("Placements_Get", new
            {
                id
            });
        }

        [HttpGet]
        [Route("placement-information/{id?}", Name = "Placements_Get")]
        public async Task<IActionResult> Placements(int id)
        {
            var dto = await _opportunityService.GetOpportunity(id);

            var viewModel = new PlacementInformationViewModel
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
        [Route("placement-information/{id?}", Name = "Placements_Post")]
        public async Task<IActionResult> Placements(PlacementInformationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
                return View(viewModel);

            viewModel.ModifiedBy = HttpContext.User.GetUserName();

            await _opportunityService.SavePlacementInformation(viewModel);

            return RedirectToRoute("EmployerFind_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers/{id?}", Name = "CheckAnswers_Get")]
        public async Task<IActionResult> CheckAnswers(int id)
        {
            var viewModel = await GetCheckAnswersViewModel(id);

            return View(viewModel);
        }
        
        [HttpPost]
        [Route("check-answers/{id?}", Name = "CheckAnswers_Post")]
        public async Task<IActionResult> CheckAnswers(CheckAnswersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(await GetCheckAnswersViewModel(viewModel.OpportunityId));

            viewModel.CreatedBy = HttpContext.User.GetUserName();

            await _opportunityService.CreateReferral(viewModel);

            return RedirectToRoute("EmailsSent_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersGap_Get")]
        public async Task<IActionResult> CheckAnswersGap(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersGapViewModel
            {
                OpportunityId = dto.Id,
                PlacementInformation = GetPlacementViewModel(dto)
            };

            return View(viewModel);
        }

        [HttpPost]
        [Route("check-answers-gap/{id?}", Name = "CheckAnswersGap_Post")]
        public async Task<IActionResult> CheckAnswersGap(CheckAnswersGapViewModel viewModel)
        {
            viewModel.CreatedBy = HttpContext.User.GetUserName();

            await _opportunityService.CreateProvisionGap(viewModel);

            return RedirectToRoute("PlacementGap_Get", new { id = viewModel.OpportunityId });
        }

        [HttpGet]
        [Route("placement-gap/{id?}", Name = "PlacementGap_Get")]
        public async Task<IActionResult> PlacementGap(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(id);

            return View(new PlacementGapViewModel { EmployerContactName = opportunity.EmployerContact });
        }

        [HttpGet]
        [Route("emails-sent/{id?}", Name = "EmailsSent_Get")]
        public async Task<IActionResult> EmailsSent(int id)
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
                Route = dto.Route
            };

            return viewModel;
        }
        private async Task<CheckAnswersViewModel> GetCheckAnswersViewModel(int id)
        {
            var dto = await _opportunityService.GetOpportunityWithRoute(id);

            var viewModel = new CheckAnswersViewModel
            {
                OpportunityId = dto.Id,
                PlacementInformation = GetPlacementViewModel(dto),
                Providers = _opportunityService.GetReferrals(id),
            };

            return viewModel;
        }

        private void Validate(PlacementInformationViewModel viewModel)
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