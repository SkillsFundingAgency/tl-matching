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
    public class When_Qualification_Save_MissingQualification_Has_No_Selected_Routes
    {
        private readonly IActionResult _result;

        public When_Qualification_Save_MissingQualification_Has_No_Selected_Routes()
        {
            var qualificationService = Substitute.For<IQualificationService>();
            var providerVenueService = Substitute.For<IProviderVenueService>();
            var providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(providerVenueService, qualificationService,
                providerQualificationService, routePathService);
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
                ShortTitle = new string("Short Title"),
                Routes = new List<RouteSummaryViewModel>
                {
                    new RouteSummaryViewModel
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = false
                    }
                }
            };

            _result = controllerWithClaims.SaveMissingQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Contains_Routes_Error()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            _result.Should().NotBeNull();

            viewResult?.Model.Should().BeOfType<MissingQualificationViewModel>();
            
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["Routes"]
                .Errors
                .Should()
                .ContainSingle(error =>
                    error.ErrorMessage == "You must choose a skill area for this qualification");
        }
    }
}