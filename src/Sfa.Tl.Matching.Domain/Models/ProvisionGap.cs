﻿namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProvisionGap : BaseEntity
    {
        public int OpportunityId { get; set; }
        public virtual Opportunity Opportunity { get; set; }
    }
}