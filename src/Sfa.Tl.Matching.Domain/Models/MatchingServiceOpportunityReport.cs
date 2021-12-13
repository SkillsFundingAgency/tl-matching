using System;
using System.Text.Json.Serialization;

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
        public string LepCode { get; set; }
        public string LepName { get; set; }
        public bool? NoSuitableStudent { get; set; }
        public bool? HadBadExperience { get; set; }
        public bool? ProvidersTooFarAway { get; set; }
        public string UserName { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}