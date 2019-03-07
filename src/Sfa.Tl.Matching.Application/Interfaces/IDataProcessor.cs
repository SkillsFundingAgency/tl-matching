using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataProcessor<in TImportDto> where TImportDto : FileImportDto
    {
        void PreProcessingHandler(TImportDto fileImportDto);
        void PostProcessingHandler(TImportDto fileImportDto);
    }
}