using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders
{
    public class ValidProviderVenueDtoListBuilder
    {
        private readonly IList<ProviderVenueDto> _providerVenueDtos;

        public ValidProviderVenueDtoListBuilder(int numberOfItems)
        {
            _providerVenueDtos = new List<ProviderVenueDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                _providerVenueDtos.Add(new ProviderVenueDto
                {
                    ProviderId = 10000546,
                    Postcode = "AA1 1AA",
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }
        }

        public IList<ProviderVenueDto> Build() => _providerVenueDtos;
    }
}
