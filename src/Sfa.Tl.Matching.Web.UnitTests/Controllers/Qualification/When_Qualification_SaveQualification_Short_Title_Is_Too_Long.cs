using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Qualification
{
    public class When_Qualification_SaveQualification_Short_Title_Is_Too_Long
    {
        private readonly IActionResult _result;

        public When_Qualification_SaveQualification_Short_Title_Is_Too_Long()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(RouteViewModelMapper).Assembly));
            var mapper = new Mapper(config);

            var qualificationService = Substitute.For<IQualificationService>();
            var providerVenueService = Substitute.For<IProviderVenueService>();
            var providerQualificationService = Substitute.For<IProviderQualificationService>();
            var routePathService = Substitute.For<IRoutePathService>();

            var qualificationController = new QualificationController(mapper,
                providerVenueService, qualificationService,
                providerQualificationService, routePathService);
            var controllerWithClaims = new ClaimsBuilder<QualificationController>(qualificationController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new SaveQualificationViewModel
            {
                QualificationId = 1,
                Title = "Qualification title",
                ShortTitle = new string('X', 101),
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
        public void Then_Model_Is_Of_Type_QualificationSearchViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<QualificationSearchViewModel>();
        }

        [Fact]
        public void Then_Model_Contains_Short_Title_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["ShortTitle"]
                .Errors
                .Should()
                .ContainSingle(error =>
                    error.ErrorMessage == "You must enter a short title that is 100 characters or fewer");
        }

        [Fact]
        public void Then_Json_Result_Is_Returned()
        {
            var result = _result as JsonResult;
            result.Should().NotBeNull();

            var validJson = result?.Value.ToString()
                .Replace("=", ":")
                .Replace(" False", "\"False\"")
                .Replace(" True", "\"True\"");

            dynamic responseObject = JsonConvert.DeserializeObject(validJson);

            Assert.True(responseObject.success == "False");

            var responseString = responseObject.response.ToString() as string;
            responseString.Should().Contain("ShortTitle");
            responseString.Should().Contain("You must enter a short title that is 100 characters or fewer");
        }
    }
}