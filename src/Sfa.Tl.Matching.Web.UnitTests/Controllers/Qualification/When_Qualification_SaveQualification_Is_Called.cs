using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
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
            var providerVenueService = Substitute.For<IProviderVenueService>();
            var providerQualificationService = Substitute.For<IProviderQualificationService>();

            var routePathService = Substitute.For<IRoutePathService>();

            var routes = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "Route 1"},
                new SelectListItem {Text = "2", Value = "Route 2"}
            };

            var routeDictionary = new Dictionary<int, string>
            {
                {1, "Route 1" },
                {2, "Route 2" }
            };

            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);

            _qualificationService = Substitute.For<IQualificationService>();
            _qualificationService.GetQualificationByIdAsync(1)
                .Returns(new QualificationSearchResultViewModel
                {
                    QualificationId = 1,
                    Title = "Qualification title",
                    ShortTitle = new string('X', 100),
                    RouteIds = new List<int> { 1 }
                });

            var qualificationController = new QualificationController(providerVenueService, _qualificationService,
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
                Routes = new List<RouteSummaryViewModel>
                {
                    new RouteSummaryViewModel
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = true
                    }
                }
            };

            _result = controllerWithClaims.SaveQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateQualification_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).UpdateQualificationAsync(Arg.Any<SaveQualificationViewModel>());
        }

        [Fact]
        public void Then_Json_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            var result = _result as JsonResult;
            result.Should().NotBeNull();

            result?.Value.ToString().Should().Contain("success = True");
        }
    }
}