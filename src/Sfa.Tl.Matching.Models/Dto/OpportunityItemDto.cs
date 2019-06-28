using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityItemDto
    {
        public int OpportunityItemId { get; set; }
        public int OpportunityId { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public bool? IsReferral { get; set; }
        public int? Placements { get; set; }
        public int? SearchResultProviderCount { get; set; }
        public bool? IsSaved { get; set; }
        public bool? IsSelectedForReferral { get; set; }
        public bool? IsCompleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public virtual ICollection<ProvisionGapDto> ProvisionGap { get; set; }
        public virtual ICollection<ReferralDto> Referral { get; set; }
        public OpportunityType OpportunityType { get; set; }
    }
}