using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class MatchingServiceProviderEmployerReport
    {
        public int OpportunityItemId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderVenueName { get; set; }
        public string EmployerName { get; set; }
        public string EmployerPostCode { get; set; }
        public string RouteName { get; set; }
        public string JobRole { get; set; }
        //public bool IsSaved { get; set; }
        //public bool IsCompleted { get; set; }
        //public string Owner { get; set; }
        public int? Placements { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}