using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenue
{
    public class ProviderVenueDataProcessor : IDataProcessor<ProviderVenueFileImportDto>
    {
        private readonly IMessageQueueService _messageQueueService;

        public ProviderVenueDataProcessor(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        public void PreProcessingHandler(ProviderVenueFileImportDto fileImportDto) { }

        public void PostProcessingHandler(ProviderVenueFileImportDto fileImportDto)
        {
            _messageQueueService.Push(new GetProximityData { PostCode = fileImportDto.PostCode, UkPrn = fileImportDto.UkPrn.ToLong() });
        }
    }
}