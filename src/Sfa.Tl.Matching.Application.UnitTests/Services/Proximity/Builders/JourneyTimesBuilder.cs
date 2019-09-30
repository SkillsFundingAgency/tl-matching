using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Proximity.Builders
{
    public class JourneyTimesBuilder
    {
        public IList<JourneyInfoDto> BuildDrivingResults() =>
            new List<JourneyInfoDto>
            {
                new JourneyInfoDto
                {
                    DestinationId = 1,
                    TravelDistance = 2500,
                    TravelTime = 30 * 60
                }
            };

        public IList<JourneyInfoDto> BuildPublicTransportResults() =>
            new List<JourneyInfoDto>
            {
                new JourneyInfoDto
                {
                    DestinationId = 2,
                    TravelDistance = 2500,
                    TravelTime = 30 * 60
                }
            };
    }
}
