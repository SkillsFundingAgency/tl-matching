using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix
{
    public interface IGoogleDistanceMatrixApiClient
    {
        Task<IDictionary<int, JourneyInfoDto>> GetJourneyTimesAsync(decimal latitude, decimal longitude, IList<LocationDto> destinations, string travelMode, long arrivalTimeSeconds);
    }
}