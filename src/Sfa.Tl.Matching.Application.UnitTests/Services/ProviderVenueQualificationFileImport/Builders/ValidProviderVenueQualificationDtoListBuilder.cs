using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualificationFileImport.Builders
{
    public class ValidProviderVenueQualificationDtoListBuilder
    {
        public IList<ProviderVenueQualificationDto> Build() => new List<ProviderVenueQualificationDto>
        {
            new()
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
                SecondaryContactPhone = "01234567891",
                VenueName = "Test Provider Venue",
                VenuePostcode = "CV1 2WT",
                Town = "Coventry",
                VenueIsEnabledForReferral = true,
                VenueIsRemoved = false,
                LarId = "1234567X",
                QualificationTitle = "Full qualification title",
                QualificationShortTitle = "Short qualification title",
                QualificationIsOffered = true,
                Routes = new List<string>
                {
                "Agriculture, environmental and animal care",
                "Digital"
                }
            }
        };
    }
}
