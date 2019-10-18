using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix
{
    public interface IGoogleDistanceMatrixApiClient
    {
        Task<IDictionary<int, JourneyInfoDto>> GetJourneyTimesAsync(string originPostcode, IList<LocationDto> destinations, string travelMode, long arrivalTimeSeconds);
    }
}