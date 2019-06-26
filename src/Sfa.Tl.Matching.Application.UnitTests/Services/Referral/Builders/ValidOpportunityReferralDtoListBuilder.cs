using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidOpportunityReferralDtoListBuilder
    {
        public IList<OpportunityReferralDto> Build() => new List<OpportunityReferralDto>
        {
            new OpportunityReferralDto
            {
                OpportunityId = 1,
                ReferralId = 1,
                ProviderName = "Provider",
                ProviderPrimaryContact = "Provider Contact",
                ProviderPrimaryContactEmail = "primary.contact@provider.co.uk",
                ProviderSecondaryContactEmail = "secondary.contact@provider.co.uk",
                EmployerName = "Employer",
                EmployerContact = "Employer Contact",
                EmployerContactPhone = "020 123 4567",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                SearchRadius = 10,
                Postcode = "AA1 1AA",
                JobRole = "Testing Job Title",
                ProviderVenuePostcode = "AA2 2AA",
                PlacementsKnown = false,
                Placements = 3,
                RouteName = "Agriculture, environmental and animal care",
                CreatedBy = "CreatedBy"
            }
        };
    }
}
