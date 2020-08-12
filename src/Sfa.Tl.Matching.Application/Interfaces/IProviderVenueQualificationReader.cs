using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationReader
    {
        ProviderVenueQualificationReadResultDto ReadData(ProviderVenueQualificationFileImportDto fileImportDto);
    }
}
