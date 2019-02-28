using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Results_Post_Is_Called
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_Results_Post_Is_Called()
        {
            var logger = Substitute.For<ILogger<ProviderController>>();
            var mapper = Substitute.For<IMapper>();

            var providerService = Substitute.For<IProviderService>();
            var routePathService = Substitute.For<IRoutePathService>();
            var providerController = new ProviderController(logger, mapper, routePathService, providerService);

            var viewModel = new SearchParametersViewModel();

            _result = providerController.Results(viewModel);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<SearchViewModel>();
        }
    }
}