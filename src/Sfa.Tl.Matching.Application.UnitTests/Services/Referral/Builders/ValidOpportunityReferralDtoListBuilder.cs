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
                ProviderDisplayName = "Provider display name",
                ProviderPrimaryContact = "Provider Contact",
                ProviderPrimaryContactEmail = "primary.contact@provider.co.uk",
                ProviderSecondaryContactEmail = "secondary.contact@provider.co.uk",
                CompanyName = "Company",
                EmployerContact = "Employer Contact",
                EmployerContactPhone = "020 123 4567",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                SearchRadius = 10,
                DistanceFromEmployer = "3.5",
                Postcode = "AA1 1AA",
                Town = "Town",
                JobRole = "Testing Job Title",
                ProviderVenueName = "Venue name",
                ProviderVenuePostcode = "AA2 2AA",
                ProviderVenueTown = "Venuetown",
                PlacementsKnown = false,
                Placements = 1,
                RouteName = "Agriculture, environmental and animal care",
                CreatedBy = "CreatedBy"
            }
        };
    }
}
