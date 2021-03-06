﻿using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityReferralDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int ReferralId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderPrimaryContact { get; set; }
        public string ProviderPrimaryContactEmail { get; set; }
        public string ProviderSecondaryContact { get; set; }
        public string ProviderSecondaryContactEmail { get; set; }
        public string ProviderVenueName { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public string ProviderVenueTown { get; set; }
        public string RouteName { get; set; }
        public short SearchRadius { get; set; }
        public string DistanceFromEmployer { get; set; }
        public string JobRole { get; set; }
        public string Postcode { get; set; }
        public string Town { get; set; }
        public string CompanyName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string CreatedBy { get; set; }

        public string VenueText
        {
            get
            {
                var venueText = string.Empty;
                if (ProviderVenueName != ProviderVenuePostcode)
                    venueText =
                        $"at {ProviderDisplayExtensions.GetProviderEmailDisplayText(ProviderVenueName, ProviderVenuePostcode, ProviderDisplayName)} ";

                venueText += $"in {ProviderVenueTown} {ProviderVenuePostcode}";

                return venueText;
            }
        }
    }
}