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
            Placements = 1,
            RouteName = "Agriculture, environmental and animal care",
            Postcode = "AA1 1AA",
            WorkplaceDetails = new List<WorkplaceDto>
            {
                new WorkplaceDto
                {
                    Placements = 2,
                    JobRole = "Job Role",
                    PlacementsKnown = true,
                    WorkplaceTown = "WorkplaceTown",
                    WorkplacePostcode = "WorkplacePostcode",
                    ProviderAndVenueDetails = new List<ProviderReferralDto>
                    {
                        new ProviderReferralDto
                        {
                            ProviderName = "Test Provider",
                            ProviderVenueTown = "ProviderTown",
                            ProviderVenuePostCode = "ProviderPostcode"
                        }
                    }
                }
            },
            CreatedBy = "CreatedBy"
        };
    }
}
