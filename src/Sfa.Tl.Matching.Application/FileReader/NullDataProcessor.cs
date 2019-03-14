using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class NullDataProcessor<TImportDto> : IDataProcessor<TImportDto> where TImportDto : FileImportDto
    {
        public void PreProcessingHandler(TImportDto fileImportDto) { }

        public void PostProcessingHandler(TImportDto fileImportDto) { }
    }
}