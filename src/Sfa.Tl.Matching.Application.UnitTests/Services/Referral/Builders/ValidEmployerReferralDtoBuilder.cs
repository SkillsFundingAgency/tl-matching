using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders
{
    public class ValidEmployerReferralDtoBuilder
    {
        private readonly EmployerReferralDto _dto;

        public ValidEmployerReferralDtoBuilder()
        {
            _dto = new EmployerReferralDto()
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
                                ProviderDisplayName = "Display Name",
                                ProviderVenueName = "Venue Name",
                                ProviderVenueTown = "ProviderTown",
                                ProviderPrimaryContact = "Primary Contact",
                                ProviderPrimaryContactEmail = "primary.contact@provider.ac.uk",
                                ProviderPrimaryContactPhone = "020 123 3210",
                                ProviderSecondaryContact = null,
                                ProviderSecondaryContactEmail = null,
                                ProviderSecondaryContactPhone = null,
                                ProviderVenuePostcode = "ProviderPostcode"
                            }
                        }
                    }
                },
                CreatedBy = "CreatedBy"
            };
        }

        public EmployerReferralDto Build() => _dto;

        public ValidEmployerReferralDtoBuilder AddSecondaryContact(bool includePhone = true, bool includeEmail = true)
        {
            var provider = _dto.WorkplaceDetails.First().ProviderAndVenueDetails.First();

            provider.ProviderSecondaryContact = "Secondary Contact";
            provider.ProviderSecondaryContactPhone = includePhone ? "021 456 0987" : null;
            provider.ProviderSecondaryContactEmail = includeEmail ? "secondary.contact@provider.ac.uk" : null;

            return this;
        }
    }
}
