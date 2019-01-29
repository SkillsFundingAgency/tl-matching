using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class SearchParametersViewModelMapper : ISearchParametersViewModelMapper
    {
        private readonly IRoutePathService _routePathLookupService;

        public SearchParametersViewModelMapper(IRoutePathService routePathLookupService)
        {
            _routePathLookupService = routePathLookupService;
        }

        public SearchParametersViewModel Populate(string selectedRouteId = null, string postcode = null)
        {
            var routes = _routePathLookupService
                .GetRoutes()
                .OrderBy(r => r.Name);

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = new List<SelectListItem>
                (
                    routes.Select(
                        r => new SelectListItem
                        {
                            Value = r.Id.ToString(),
                            Text = r.Name
                        })
                ),
                SelectedRouteId = selectedRouteId,
                Postcode = postcode
            };

            if (viewModel.SelectedRouteId == null && viewModel.RoutesSelectList.Count > 0)
            {
                viewModel.SelectedRouteId = viewModel.RoutesSelectList.First().Value;
            }

            return viewModel;
        }
    }
}
