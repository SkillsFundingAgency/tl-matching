using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_MissingQualification_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;

        public When_Qualification_MissingQualification_Is_Loaded()
        {
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1", Summary = "Route Summary 1"},
                new Route {Id = 2, Name = "Route 2", Summary = "Route Summary 2"}
            }.AsQueryable();
            
            var config = new MapperConfiguration(c => c.AddMaps(typeof(RouteViewModelMapper).Assembly));
            var mapper = new Mapper(config);

             _qualificationService = Substitute.For<IQualificationService>();

            _qualificationService.GetLarTitleAsync("12345678")
                .Returns("LAR title from lookup");

            var providerVenueService = Substitute.For<IProviderVenueService>();
            providerVenueService.GetVenuePostcodeAsync(1)
                .Returns("CV1 2WT");

            _routePathService = Substitute.For<IRoutePathService>();
            _routePathService.GetRoutes().Returns(routes);

            var providerQualificationService = Substitute.For<IProviderQualificationService>();

            var qualificationController = new QualificationController(mapper, providerVenueService, _qualificationService, providerQualificationService, _routePathService);

            _result = qualificationController.MissingQualificationAsync(1, "12345678")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetRoutes_Is_Called_Exactly_Once()
        {
            _routePathService.Received(1).GetRoutes();
        }

        [Fact]
        public void Then_GetLarTitleAsync_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).GetLarTitleAsync("12345678");
        }
        
        [Fact]
        public void Then_ViewModel_Fields_Are_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();

            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<MissingQualificationViewModel>();
            viewModel.Should().NotBeNull();
            viewModel.ProviderVenueId.Should().Be(1);
            viewModel.LarId.Should().Be("12345678");
            viewModel.Title.Should().BeEquivalentTo("LAR title from lookup");
            viewModel.Routes.Should().NotBeNullOrEmpty();
        
            var routes = _result.GetViewModel<MissingQualificationViewModel>().Routes;
            
            routes.Count.Should().Be(2);
            
            routes.Should().Contain(r => r.Id == 1 && r.Name == "Route 1");
            routes.Should().Contain(r => r.Id == 2 && r.Name == "Route 2");
            
            routes.Single(r => r.Id == 1).Summary.Should().Be("Route Summary 1");
            routes.Single(r => r.Id == 2).Summary.Should().Be("Route Summary 2");
        }
    }
}