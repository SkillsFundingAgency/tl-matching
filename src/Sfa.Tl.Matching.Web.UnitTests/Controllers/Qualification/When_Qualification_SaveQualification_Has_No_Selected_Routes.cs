using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_SaveQualification_Has_No_Selected_Routes
    {
        private readonly IActionResult _result;

        public When_Qualification_SaveQualification_Has_No_Selected_Routes()
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

            var viewModel = new SaveQualificationViewModel
            {
                QualificationId = 1,
                Title = "Qualification title",
                ShortTitle = new string("Short Title"),
                Source = "Test",
                Routes = new List<RouteSummaryViewModel>
                {
                    new()
                    {
                        Id = 1,
                        Name = "Route 1",
                        IsSelected = false
                    }
                }
            };

            _result = controllerWithClaims.SaveQualificationAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Json_Result_Is_Returned_With_Short_Title_Error()
        {
            var result = _result as JsonResult;
            result.Should().NotBeNull();
            
            var success = result.GetValue<bool>("success");
            var response = result.GetValue<string>("response");

            success.Should().BeFalse();
            response.Should().Contain("Routes");
            response.Should().Contain("You must choose a skill area for this qualification");
        }
    }
}