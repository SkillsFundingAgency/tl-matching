using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Route> _routeRepository;

        public RoutePathService(IMapper mapper, IRepository<Route> routeRepository)
        {
            _mapper = mapper;
            _routeRepository = routeRepository;
        }

        public async Task<IDictionary<int, string>> GetRouteDictionaryAsync()
        {
            return await _routeRepository.GetManyAsync().OrderBy(r => r.Name).ToDictionaryAsync(r => r.Id, r => r.Name);
        }

        public async Task<IList<int>> GetRouteIdsAsync()
        {
            return await _routeRepository.GetManyAsync().Select(r => r.Id).OrderBy(r => r).ToListAsync();
        }

        public async Task<IList<SelectListItem>> GetRouteSelectListItemsAsync()
        {
            return await _routeRepository.GetManyAsync().OrderBy(r => r.Name).ProjectTo<SelectListItem>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<IList<RouteSummaryViewModel>> GetRouteSummaryAsync()
        {
            return await _routeRepository
                .GetManyAsync()
                .OrderBy(r => r.Name)
                .ProjectTo<RouteSummaryViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<RouteSummaryViewModel> GetRouteSummaryByNameAsync(string name)
        {
            var route = await _routeRepository.GetSingleOrDefaultAsync(r => r.Name == name);
            var routeSummaryViewModel = _mapper.Map<Route, RouteSummaryViewModel>(route);
            return routeSummaryViewModel;
        }
    }
}
