using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderProximityService : IProviderProximityService
    {
        private readonly ISearchProvider _searchProvider;

        public ProviderProximityService(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }
    }
}