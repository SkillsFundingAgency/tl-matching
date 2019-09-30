﻿using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class MatchingServiceOpportunityReport
    {
        public int OpportunityItemId { get; set; }
        public string OpportunityType { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsSaved { get; set; }
        public bool? PipelineOpportunity { get; set; }
        public Guid? EmployerCrmId { get; set; }
        public string CompanyName { get; set; }
        public string Aupa { get; set; }
        public string Owner { get; set; }
        public string EmployerPostCodeEnteredInSearch { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string JobRole { get; set; }
        public int? ProviderCount { get; set; }
        public string RouteName { get; set; }
        public string Region { get; set; }
        public string Team { get; set; }
        public string UserName { get; set; }
        public DateTime? WeekEndDate { get; set; }
        public string Date { get; set; }
    }
}