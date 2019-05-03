using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenue
{
    public class ProviderVenueDataProcessor : IDataProcessor<Domain.Models.ProviderVenue>
    {
        private readonly IMessageQueueService _messageQueueService;

        public ProviderVenueDataProcessor(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        public void PreProcessingHandler(IList<Domain.Models.ProviderVenue> entities) { }

        public void PostProcessingHandler(IList<Domain.Models.ProviderVenue> entities)
        {
            foreach (var providerVenue in entities)
            {
                _messageQueueService.PushProximityDataAsync(new GetProximityData { Postcode = providerVenue.Postcode, ProviderVenueId = providerVenue.Id });
            }
        }
    }
}