﻿// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.AdminUser)]
#endif
    public class QualificationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IRoutePathService _routePathService;

        public QualificationController(
            IMapper mapper,
            IProviderVenueService providerVenueService,
            IQualificationService qualificationService,
            IProviderQualificationService providerQualificationService,
            IRoutePathService routePathService)
        {
            _mapper = mapper;
            _providerVenueService = providerVenueService;
            _qualificationService = qualificationService;
            _providerQualificationService = providerQualificationService;
            _routePathService = routePathService;
        }

        [Route("add-qualification/{providerVenueId}", Name = "AddQualification")]
        public async Task<IActionResult> AddQualification(int providerVenueId)
        {
            var postcode = await _providerVenueService.GetVenuePostcodeAsync(providerVenueId);

            return View(new AddQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                Postcode = postcode
            });
        }

        [HttpPost]
        [Route("add-qualification/{providerId}", Name = "CreateQualification")]
        public async Task<IActionResult> AddQualification(AddQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var isValid = await _qualificationService.IsValidLarIdAsync(viewModel.LarId);

            if (string.IsNullOrWhiteSpace(viewModel.LarId) || !isValid)
            {
                ModelState.AddModelError("LarId", "Enter a learning aim reference (LAR) that has 8 characters");
                return View(viewModel);
            }

            var qualification = await _qualificationService.GetQualificationAsync(viewModel.LarId);

            if (qualification == null)
            {
                var isValidLar = await _qualificationService.IsValidOfqualLarIdAsync(viewModel.LarId);
                if (!isValidLar)
                {
                    ModelState.AddModelError("LarId", "You must enter a real learning aim reference (LAR)");
                    return View(viewModel);
                }

                return RedirectToRoute("MissingQualification",
                    new
                    {
                        providerVenueId = viewModel.ProviderVenueId,
                        larId = viewModel.LarId
                    });
            }

            viewModel.QualificationId = qualification.Id;
            await _providerQualificationService.CreateProviderQualificationAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail", 
                new { providerVenueId = viewModel.ProviderVenueId });
        }
        
        [Route("edit-qualifications", Name = "EditQualifications")]
        public IActionResult EditQualifications()
        {
            return View(new QualificationSearchViewModel());
        }

        [HttpPost]
        [Route("edit-qualifications", Name = "SearchQualifications")]
        public async Task<IActionResult> EditQualifications(QualificationSearchViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var searchResult = await _qualificationService.SearchQualification(viewModel.Title);

            return View(searchResult);
        }
        
        [HttpPost]
        [Route("save-qualification", Name = "SaveQualification")]
        public async Task<IActionResult> SaveQualification(SaveQualificationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                //viewModel.Routes = GetRoutes(viewModel);
                return View("EditQualifications", new QualificationSearchViewModel
                {
                        Title = viewModel.SearchString
                });
            }

            await _qualificationService.UpdateQualificationAsync(viewModel);

            return RedirectToRoute("EditQualifications"); 
            //    new QualificationSearchViewModel
            //    {
            //        Title = viewModel.SearchString
            //    });
        }

        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "MissingQualification")]
        public async Task<IActionResult> MissingQualification(int providerVenueId, string larId)
        {
            //Get title from service, based on LAR
            var title = await _qualificationService.GetLarTitleAsync(larId);

            return View(new MissingQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                LarId = larId,
                QualificationId = 1,
                Title = title,
                Routes = GetRoutes()
            });
        }

        [HttpPost]
        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "SaveMissingQualification")]
        public async Task<IActionResult> MissingQualification(MissingQualificationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Routes = GetRoutes(viewModel);
                return View(viewModel);
            }
            
            var qualificationId = await _qualificationService.CreateQualificationAsync(viewModel);

            await _providerQualificationService.CreateProviderQualificationAsync(
                new AddQualificationViewModel
                {
                    ProviderVenueId = viewModel.ProviderVenueId,
                    QualificationId = qualificationId,
                    Source = viewModel.Source
                });

            return RedirectToRoute("GetProviderVenueDetail",
                new { providerVenueId = viewModel.ProviderVenueId });
        }

        [Route("remove-qualification/{providerVenueId}/{qualificationId}", Name = "RemoveQualification")]
        public async Task<IActionResult> RemoveQualification(int providerVenueId, int qualificationId)
        {
            await _providerQualificationService.RemoveProviderQualificationAsync(providerVenueId, qualificationId);

            return RedirectToRoute("GetProviderVenueDetail", new {providerVenueId });
        }

        private IList<RouteViewModel> GetRoutes(MissingQualificationViewModel viewModel = null)
        {
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();

            var routesList = _mapper.Map<RouteViewModel[]>(routes);

            foreach (var route in routesList)
            {
                if (viewModel?.Routes.Any(r => r.Id == route.Id && r.IsSelected) == true)
                {
                    route.IsSelected = true;
                }
            }

            return routesList;
        }

        private void Validate(MissingQualificationViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ShortTitle) || viewModel.ShortTitle.Length > 100)
            {
                ModelState.AddModelError("ShortTitle", "You must enter a short title that is 100 characters or fewer");
            }

            if (!viewModel.Routes.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Routes", "You must choose a skill area for this qualification");
            }
        }

        private void Validate(SaveQualificationViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ShortTitle) || viewModel.ShortTitle.Length > 100)
            {
                ModelState.AddModelError("ShortTitle", "You must enter a short title that is 100 characters or fewer");
            }

            //TODO: Validate route ids. Taken out to test intial save
            //if (viewModel.Routes == null || !viewModel.Routes.Any(r => r.IsSelected))
            //{
            //    ModelState.AddModelError("Routes", "You must choose a skill area for this qualification");
            //}
        }
    }
}