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
                new Route {Id = 1, Name = "Route 1"},
                new Route {Id = 2, Name = "Route 2"}
            }.AsQueryable();

            var paths = new List<Path>
            {
                new Path {Id = 1, RouteId = 1, Name = "Path 1"},
                new Path {Id = 2, RouteId = 2, Name = "Path 2"},
                new Path {Id = 3, RouteId = 2, Name = "Path 3"}
            }.AsQueryable();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(RouteViewModelMapper).Assembly));
            var mapper = new Mapper(config);

             _qualificationService = Substitute.For<IQualificationService>();

            _qualificationService.GetLarTitleAsync("12345678")
                .Returns("LAR title from lookup");

            var providerVenueService = Substitute.For<IProviderVenueService>();
            providerVenueService.GetVenuePostcodeAsync(1)
                .Returns("CV1 2WT");

            _routePathService = Substitute.For<IRoutePathService>();
            _routePathService.GetRoutes().Returns(routes);
            _routePathService.GetPaths().Returns(paths);

            var providerQualificationService = Substitute.For<IProviderQualificationService>();

            var qualificationController = new QualificationController(mapper, providerVenueService, _qualificationService, providerQualificationService, _routePathService);

            _result = qualificationController.MissingQualification(1, "12345678")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetRoutes_Is_Called_Exactly_Once()
        {
            _routePathService.Received(1).GetRoutes();
        }
        
        [Fact]
        public void Then_GetPaths_Is_Called_Exactly_Once()
        {
            _routePathService.Received(1).GetPaths();
        }


        [Fact]
        public void Then_GetLarTitleAsync_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).GetLarTitleAsync("12345678");
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ViewModel_Fields_Are_Set()
        {
            var viewModel = _result.GetViewModel<MissingQualificationViewModel>();
            viewModel.ProviderVenueId.Should().Be(1);
            viewModel.LarId.Should().Be("12345678");
            viewModel.Title.Should().BeEquivalentTo("LAR title from lookup");
            viewModel.Routes.Should().NotBeNullOrEmpty();
        }
        
        [Fact]
        public void Then_ViewModel_Routes_Are_Set()
        {
            var routes = _result.GetViewModel<MissingQualificationViewModel>().Routes;

            routes.Count.Should().Be(2);
            
            routes.Should().Contain(r => r.Id == 1 && r.Name == "Route 1");
            routes.Should().Contain(r => r.Id == 2 && r.Name == "Route 2");
            
            routes.Single(r => r.Id == 1).PathNames.Should().Contain("Path 1");
            routes.Single(r => r.Id == 2).PathNames.Should().Contain("Path 3");
            routes.Single(r => r.Id == 2).PathNames.Should().Contain("Path 3");
        }

    }
}