using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NSubstitute;
using Sfa.Tl.Matching.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Search
{
    public class When_Search_Controller_Index_Is_Loaded
    {
        private SearchController _controller;
        private ISearchParametersViewModelMapper _viewModelMapper;
        private SearchParametersViewModel _viewModel;
        private IActionResult _result;
        private ILogger<SearchController> _logger;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" }
                }
                .AsQueryable();

            _viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = new List<SelectListItem>
                (
                    routes.Select(
                        r => new SelectListItem {Value = r.Name, Text = r.Name})
                ),
                SelectedRouteId = "1"
            };

            _viewModelMapper = Substitute.For<ISearchParametersViewModelMapper>();
            _viewModelMapper.Populate(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_viewModel);

            _logger = Substitute.For<ILogger<SearchController>>();

            _controller = new SearchController(_viewModelMapper, _logger);
            _result = _controller.Index(null, null);
        }
        
        [Test]
        public void Then_View_Model_Mapper_Populate_Is_Called_Exactly_Once() =>
            _viewModelMapper.Received(1)
                .Populate(Arg.Any<string>(), Arg.Any<string>());

        [Test]
        public void Then_Result_Contains_ViewModel()
        {
            var view = _result as ViewResult;
            var viewModel = view?.Model as SearchParametersViewModel;
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}