using System.Collections.Generic;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderQualification
{
    public class ProviderQualificationDataParser : IDataParser<ProviderQualificationDto>
    {
        public IEnumerable<ProviderQualificationDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is ProviderQualificationFileImportDto data)) return null;

            var providerQualificationDto = new ProviderQualificationDto
            {
                ProviderVenueId = data.ProviderVenueId,
                QualificationId = data.QualificationId,
                NumberOfPlacements = data.NumberOfPlacements.ToInt(),
                Source = data.Source,
                CreatedBy = data.CreatedBy
            };

            return new List<ProviderQualificationDto> { providerQualificationDto };
        }
    }
}