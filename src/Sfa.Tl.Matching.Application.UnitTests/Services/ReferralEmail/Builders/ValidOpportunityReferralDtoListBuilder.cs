using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail.Builders
{
    public class ValidOpportunityReferralDtoListBuilder
    {
        public IList<OpportunityReferralDto> BuildWithOneReferral() => new List<OpportunityReferralDto>
        {
            new()
            {
                OpportunityId = 1,
                ReferralId = 1,
                ProviderName = "Provider",
                ProviderDisplayName = "Provider display name",
                ProviderPrimaryContact = "Provider Contact",
                ProviderPrimaryContactEmail = "primary.contact@provider.co.uk",
                ProviderSecondaryContact = "Provider Secondary Contact",
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

        public IList<OpportunityReferralDto> BuildWithMultipleReferrals() => new List<OpportunityReferralDto>
        {
            new()
            {
                OpportunityId = 1,
                OpportunityItemId = 1,
                ProviderPrimaryContact = "contact",
                ProviderName = "Name",
                RouteName = "Routename",
                ProviderVenueTown = "Provider town",
                ProviderVenuePostcode = "Provider postcode",
                DistanceFromEmployer = "3.5",
                JobRole = "Job role",
                CompanyName = "Companyname",
                EmployerContact = "Employer contact",
                EmployerContactPhone = "Employer phone",
                EmployerContactEmail = "Employer email",
                Town = "Town",
                Postcode =  "Postcode",
                Placements =  1
            },
            new()
            {
                OpportunityId = 1,
                OpportunityItemId = 2,
                ProviderPrimaryContact = "contact",
                ProviderName = "Name",
                RouteName = "Routename",
                ProviderVenueTown = "Provider town",
                ProviderVenuePostcode = "Provider postcode",
                DistanceFromEmployer = "3.5",
                JobRole = "Job role",
                CompanyName = "Companyname",
                EmployerContact = "Employer contact",
                EmployerContactPhone = "Employer phone",
                EmployerContactEmail = "Employer email",
                Town = "Town",
                Postcode =  "Postcode",
                Placements =  1
            },
            new()
            {
                OpportunityId = 1,
                OpportunityItemId = 3,
                ProviderPrimaryContact = "contact",
                ProviderName = "Name",
                RouteName = "Routename",
                ProviderVenueTown = "Provider town",
                ProviderVenuePostcode = "Provider postcode",
                DistanceFromEmployer = "3.5",
                JobRole = "Job role",
                CompanyName = "Companyname",
                EmployerContact = "Employer contact",
                EmployerContactPhone = "Employer phone",
                EmployerContactEmail = "Employer email",
                Town = "Town",
                Postcode =  "Postcode",
                Placements =  1
            },
            new()
            {
                OpportunityId = 1,
                OpportunityItemId = 4,
                ProviderPrimaryContact = "contact",
                ProviderName = "Name",
                RouteName = "Routename",
                ProviderVenueTown = "Provider town",
                ProviderVenuePostcode = "Provider postcode",
                DistanceFromEmployer = "3.5",
                JobRole = "Job role",
                CompanyName = "Companyname",
                EmployerContact = "Employer contact",
                EmployerContactPhone = "Employer phone",
                EmployerContactEmail = "Employer email",
                Town = "Town",
                Postcode =  "Postcode",
                Placements =  1
            }
        };
    }
}
