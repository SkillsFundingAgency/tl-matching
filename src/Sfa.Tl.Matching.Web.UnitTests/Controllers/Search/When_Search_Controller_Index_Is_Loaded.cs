using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Search
{
    public class When_Search_Controller_Index_Is_Loaded
    {
        private readonly IActionResult _result;

        public When_Search_Controller_Index_Is_Loaded()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" }
                }
                .AsQueryable();

            var logger = Substitute.For<ILogger<SearchController>>();
            var mapper = Substitute.For<IMapper>();

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);
            var searchController = new SearchController(logger, mapper, routePathService);

            _result = searchController.Index();
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

        // TODO Check ViewModel has everything set up

        private SearchParametersViewModel GetViewModel()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as SearchParametersViewModel;

            return viewModel;
        }
    }
}