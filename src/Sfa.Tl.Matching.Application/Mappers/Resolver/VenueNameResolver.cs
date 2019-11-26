using AutoMapper;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers.Resolver
{
    public class VenueNameResolver : IValueResolver<ProviderVenueDetailViewModel, ProviderVenue, string>
    {
        private readonly ILocationApiClient _locationApiClient;

        public VenueNameResolver(ILocationApiClient locationApiClient)
        {
            _locationApiClient = locationApiClient;
        }

        public string Resolve(ProviderVenueDetailViewModel source, ProviderVenue destination, string destMember, ResolutionContext context)
        {
            var name = source.Name;

            var (includeTerminated, postcode) = _locationApiClient.IsValidPostcodeAsync(name, true).GetAwaiter().GetResult();
            
            return includeTerminated ? postcode : name.ToTitleCase();
        }
    }
}
