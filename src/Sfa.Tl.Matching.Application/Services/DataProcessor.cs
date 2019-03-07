using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DataProcessor<TImportDto> : IDataProcessor<TImportDto> where TImportDto : FileImportDto
    {
        public void PreProcessingHandler(TImportDto fileImportDto)
        {
            throw new System.NotImplementedException();
        }

        public void PostProcessingHandler(TImportDto fileImportDto)
        {
            throw new System.NotImplementedException();
        }
    }
}