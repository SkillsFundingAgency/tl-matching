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
            _dto = new EmployerReferralDto
            {
                OpportunityId = 1,
                CompanyName = "Employer",
                PrimaryContact = "Employer Contact",
                Phone = "020 123 4567",
                Email = "employer.contact@employer.co.uk",
                JobRole = "Testing Job Title",
                PlacementsKnown = false,
                Placements = 1,
                RouteName = "Agriculture, environmental and animal care",
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
                                DisplayName = "Display Name",
                                ProviderVenueName = "Venue Name",
                                Town = "ProviderTown",
                                PrimaryContact = "Primary Contact",
                                PrimaryContactEmail = "primary.contact@provider.ac.uk",
                                PrimaryContactPhone = "020 123 3210",
                                SecondaryContact = null,
                                SecondaryContactEmail = null,
                                SecondaryContactPhone = null,
                                Postcode = "ProviderPostcode"
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

            provider.SecondaryContact = "Secondary Contact";
            provider.SecondaryContactPhone = includePhone ? "021 456 0987" : null;
            provider.SecondaryContactEmail = includeEmail ? "secondary.contact@provider.ac.uk" : null;

            return this;
        }
    }
}
