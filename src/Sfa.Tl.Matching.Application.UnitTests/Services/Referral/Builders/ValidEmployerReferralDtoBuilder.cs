using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidEmployerReferralDtoBuilder
    {
        public EmployerReferralDto Build() => new EmployerReferralDto
        {
            OpportunityId = 1,
            CompanyName = "Employer",
            EmployerContact = "Employer Contact",
            EmployerContactPhone = "020 123 4567",
            EmployerContactEmail = "employer.contact@employer.co.uk",
            JobRole = "Testing Job Title",
            PlacementsKnown = false,
            Placements = 3,
            RouteName = "Agriculture, environmental and animal care",
            Postcode = "AA1 1AA",
            ProviderReferralInfo = new List<ProviderReferralInfoDto>
            {
                new ProviderReferralInfoDto
                {
                    ReferralId = 1,
                    ProviderName = "Provider",
                    ProviderPrimaryContact = "Provider Contact",
                    ProviderPrimaryContactEmail = "primary.contact@provider.co.uk",
                    ProviderPrimaryContactPhone = "01777757777",
                    ProviderVenuePostcode = "AA2 2AA",
                    QualificationShortTitles = new List<string>
                    {
                        "Qualification 1",
                        "Qualification 2"
                    }
                }
            },
            CreatedBy = "CreatedBy"
        };
    }
}
