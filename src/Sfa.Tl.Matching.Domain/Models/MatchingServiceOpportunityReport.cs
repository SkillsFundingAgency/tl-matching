using System;
using Newtonsoft.Json;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class MatchingServiceOpportunityReport
    {
        [JsonIgnore]
        public int OpportunityItemId { get; set; }
        public string OpportunityType { get; set; }
        public bool? PipelineOpportunity { get; set; }
        public int? EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string Aupa { get; set; }
        public string Owner { get; set; }
        public string EmployerPostCodeEnteredInSearch { get; set; }
        public int? Placements { get; set; }
        public string JobRole { get; set; }
        public string RouteName { get; set; }
        public string UserName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}