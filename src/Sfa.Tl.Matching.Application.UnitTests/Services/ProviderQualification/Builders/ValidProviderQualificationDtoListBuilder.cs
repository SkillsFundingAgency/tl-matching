using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQualification.Builders
{
    public class ValidProviderQualificationDtoListBuilder
    {
        private readonly IList<ProviderQualificationDto> _providerQualificationDtos;

        public ValidProviderQualificationDtoListBuilder(int numberOfItems)
        {
            _providerQualificationDtos = new List<ProviderQualificationDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                _providerQualificationDtos.Add(new ProviderQualificationDto
                {
                    ProviderVenueId = 10000546,
                    QualificationId = 1,
                    NumberOfPlacements = 1,
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }
        }

        public IList<ProviderQualificationDto> Build() => _providerQualificationDtos;
    }
}
