using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_Save_MissingQualification_Is_Called
    {
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;
        private readonly IActionResult _result;

        public When_Qualification_Save_MissingQualification_Is_Called()
        {
            _qualificationService = Substitute.For<IQualificationService>();
            var providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(providerVenueService, _qualificationService,
                _providerQualificationService, routePathService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new MissingQualificationViewModel
            {
                ProviderVenueId = 1,
                LarId = "12345678",
                QualificationId = 1,
                Title = "Qualification title",
                ShortTitle = new string('X', 100),
                Routes = new List<RouteSummaryViewModel>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = true
                    }
                }
            };

            _result = controllerWithClaims.SaveMissingQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_RedirectToRoute()
        {
            _result.Should().NotBeNull();
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("GetProviderVenueDetail");
            result?.RouteValues["providerVenueId"].Should().Be(1);
        }

        [Fact]
        public void Then_CreateProviderQualification_Is_Called_Exactly_Once()
        {
            _providerQualificationService.Received(1).CreateProviderQualificationAsync(Arg.Any<AddQualificationViewModel>());
        }

        [Fact]
        public void Then_CreateQualification_Is_Called_Exactly_Once()
        {
            _qualificationService.Received(1).CreateQualificationAsync(Arg.Any<MissingQualificationViewModel>());
        }
    }
}