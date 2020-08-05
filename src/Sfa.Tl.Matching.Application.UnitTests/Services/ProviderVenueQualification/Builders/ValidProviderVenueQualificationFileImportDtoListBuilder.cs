using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualification.Builders
{
    public class ValidProviderVenueQualificationFileImportDtoListBuilder
    {
        private readonly IList<ProviderVenueQualificationDto> _providerVenueQualificationDtoList;

        public ValidProviderVenueQualificationFileImportDtoListBuilder()
        {
            _providerVenueQualificationDtoList = new List<ProviderVenueQualificationDto>
            {
                new ProviderVenueQualificationDto
                {
                    UkPrn = 10000001,
                    ProviderName = "Test Provider Name",
                    Name = "Test Provider Name",
                    DisplayName = "Test Provider Display Name",
                    InMatchingService = true,
                    IsCdfProvider = true,
                    IsEnabledForReferral = true,
                    PrimaryContact = "test primary contact",
                    PrimaryContactEmail = "testprimary@test.com",
                    PrimaryContactPhone = "01234567890",
                    SecondaryContact = "test secondary contact",
                    SecondaryContactEmail = "testsecondary@test.com",
                    SecondaryContactPhone = "01234567891"
                }
            };
        }

        public ValidProviderVenueQualificationFileImportDtoListBuilder AddVenue(bool isEnabledForReferral = false, bool isRemoved = false)
        {
            var providerVenueQualificationDto = _providerVenueQualificationDtoList.First();

            providerVenueQualificationDto.VenueName = "Test Provider Venue";
            providerVenueQualificationDto.VenuePostcode = "CV1 2WT";
            providerVenueQualificationDto.Town = "Coventry";
            providerVenueQualificationDto.VenueIsEnabledForReferral = isEnabledForReferral;
            providerVenueQualificationDto.VenueIsRemoved = isRemoved;

            return this;
        }

        public ValidProviderVenueQualificationFileImportDtoListBuilder AddQualificationWithRoutes()
        {
            var providerVenueQualificationDto = _providerVenueQualificationDtoList.First();

            providerVenueQualificationDto.LarId = "1234567X";
            providerVenueQualificationDto.VenuePostcode = "CV1 2WT";
            providerVenueQualificationDto.QualificationTitle = "Full qualification title";
            providerVenueQualificationDto.QualificationShortTitle = "Short qualification title";
            providerVenueQualificationDto.QualificationIsOffered = true;
            providerVenueQualificationDto.Routes = new List<string>
            {
                "Agriculture, environmental and animal care",
                "Digital"
            };

            return this;
        }
        public IList<ProviderVenueQualificationDto> Build() => _providerVenueQualificationDtoList;
    }
}
