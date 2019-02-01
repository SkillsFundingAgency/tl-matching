using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Web.Mappers;

namespace Sfa.Tl.Matching.Web.UnitTests.Mappers.SearchParametersViewModel
{
    public class When_SearchParametersViewModel_Is_Populated
    {
        private IQueryable<Route> _routes;
        private IRoutePathService _routePathLookupService;
        private ViewModels.SearchParametersViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Good Route" },
                    new Route { Id = 2, Name = "Another Route" }
                }
                .AsQueryable();

            _routePathLookupService =
                Substitute
                    .For<IRoutePathService>();
            _routePathLookupService.GetRoutes().Returns(_routes);
            
            var mapper = new SearchParametersViewModelMapper(_routePathLookupService);
            _viewModel = mapper.Populate(null, null);
        }

        [Test]
        public void Then_ViewModel_IsNotNull() =>
            Assert.NotNull(_viewModel);

        [Test]
        public void Then_RoutesSelectList_Is_NotNull() =>
            Assert.NotNull(_viewModel.RoutesSelectList);

        [Test]
        public void Then_RoutesSelectList_Count_Is_Correct() =>
            Assert.AreEqual(2, _viewModel.RoutesSelectList.Count);
        
        [Test]
        public void Then_RoutesSelectList_Id_Is_Mapped_To_Value_Correctly()
        {
            foreach (var route in _routes)
            {
                Assert.IsTrue(_viewModel.RoutesSelectList.Any(r => r.Value == route.Id.ToString()));
            }
        }

        [Test]
        public void Then_RoutesSelectList_Name_Is_Mapped_To_Text_Correctly()
        {
            foreach (var route in _routes)
            {
                Assert.IsTrue(_viewModel.RoutesSelectList.Any(r => r.Text == route.Name));
            }
        }

        [Test]
        public void Then_SelectedRouteId_Is_Mapped_To_The_Correctly_Ordered_Route_Id() =>
            Assert.AreEqual("2", _viewModel.SelectedRouteId);
    }
}