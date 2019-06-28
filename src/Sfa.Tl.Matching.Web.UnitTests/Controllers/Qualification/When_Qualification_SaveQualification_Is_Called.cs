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
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_SaveQualification_Is_Called
    {
        private readonly IQualificationService _qualificationService;
        private readonly IActionResult _result;

        public When_Qualification_SaveQualification_Is_Called()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(RouteViewModelMapper).Assembly));
            var mapper = new Mapper(config);

            var providerVenueService = Substitute.For<IProviderVenueService>();
            var providerQualificationService = Substitute.For<IProviderQualificationService>();

            var routePathService = Substitute.For<IRoutePathService>();
            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1", Summary = "Route Summary 1"}
            }.AsQueryable();
            routePathService.GetRoutes().Returns(routes);

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.GetQualificationByIdAsync(1)
                .Returns(new QualificationSearchResultViewModel
                {
                    QualificationId = 1,
                    Title = "Qualification title",
                    ShortTitle = new string('X', 100),
                    RouteIds = new List<int> { 1 }
                });

            var qualificationController = new QualificationController(mapper,
                providerVenueService, _qualificationService,
                providerQualificationService, routePathService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new SaveQualificationViewModel
            {
                QualificationId = 1,
                Title = "Qualification title",
                ShortTitle = new string('X', 100),
                Source = "Test",
                Routes = new List<RouteViewModel>
                {
                    new RouteViewModel
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = true
                    }
                }
            };

            _result = controllerWithClaims.SaveQualification(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_UpdateQualification_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).UpdateQualificationAsync(Arg.Any<SaveQualificationViewModel>());
        }

        [Fact]
        public void Then_Json_Result_Is_Returned()
        {
            var result = _result as JsonResult;
            result.Should().NotBeNull();

            result?.Value.ToString().Should().Contain("success = True");
        }
    }
}