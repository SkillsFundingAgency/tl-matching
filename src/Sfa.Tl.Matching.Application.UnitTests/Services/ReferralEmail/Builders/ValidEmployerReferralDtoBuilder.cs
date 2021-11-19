using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail.Builders
{
    public class ValidEmployerReferralDtoBuilder
    {
        private readonly EmployerReferralDto _dto;

        public ValidEmployerReferralDtoBuilder()
        {
            _dto = new()
            {
                OpportunityId = 1,
                CompanyName = "Employer",
                PrimaryContact = "Employer Contact",
                Phone = "020 123 4567",
                Email = "employer.contact@employer.co.uk",
                WorkplaceDetails = new List<WorkplaceDto>
                {
                    new()
                    {
                        Placements = 2,
                        JobRole = "Job Role",
                        PlacementsKnown = true,
                        WorkplaceTown = "WorkplaceTown",
                        WorkplacePostcode = "WorkplacePostcode",
                        ProviderAndVenueDetails = new List<ProviderReferralDto>
                        {
                            new()
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

        public ValidEmployerReferralDtoBuilder ClearWorkplaceDetails()
        {
            (_dto.WorkplaceDetails as List<WorkplaceDto>)?.Clear();

            return this;
        }
    }
}
