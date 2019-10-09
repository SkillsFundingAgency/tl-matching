using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Proximity.Builders
{
    public class JourneyTimesBuilder
    {
        public IDictionary<int, JourneyInfoDto> BuildDrivingResults() =>
            new Dictionary<int, JourneyInfoDto>
            {
                {
                    1, new JourneyInfoDto
                    {
                        DestinationId = 1,
                        TravelDistance = 2500,
                        TravelTime = 30 * 60
                    }
                }
            };

        public IDictionary<int, JourneyInfoDto> BuildPublicTransportResults() =>
            new Dictionary<int, JourneyInfoDto>
            {
                {
                    2, new JourneyInfoDto
                    {
                        DestinationId = 2,
                        TravelDistance = 2500,
                        TravelTime = 30 * 60
                    }
                }
            };
    }
}
