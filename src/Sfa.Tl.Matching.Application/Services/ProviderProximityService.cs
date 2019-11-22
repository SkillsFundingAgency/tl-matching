using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderProximityService : IProviderProximityService
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ILocationService _locationService;

        public ProviderProximityService(ISearchProvider searchProvider,
            ILocationService locationService)
        {
            _searchProvider = searchProvider;
            _locationService = locationService;
        }
    }
}