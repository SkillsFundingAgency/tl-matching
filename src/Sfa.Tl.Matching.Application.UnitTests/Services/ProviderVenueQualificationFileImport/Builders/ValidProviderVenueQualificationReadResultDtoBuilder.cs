using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualificationFileImport.Builders
{
    public class ValidProviderVenueQualificationReadResultDtoBuilder
    {
        public ProviderVenueQualificationReadResultDto Build() => new()
        {
            Error = null,
            ProviderVenueQualifications = new ValidProviderVenueQualificationDtoListBuilder().Build()   
        };
    }
}
