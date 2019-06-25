using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityBuilder
    {
        private readonly Domain.Models.Opportunity _opportunity;

        public OpportunityBuilder()
        {
            _opportunity = new Domain.Models.Opportunity
            {
                OpportunityItem = new List<OpportunityItem>()
            };
        }

        internal OpportunityBuilder AddOpportunityItem(OpportunityType type)
        {
            _opportunity.OpportunityItem.Add(new OpportunityItem
            {
                OpportunityType = type.ToString()
            });

            return this;
        }

        internal OpportunityBuilder AddReferralItem()
        {
            AddOpportunityItem(OpportunityType.Referral);

            return this;
        }

        internal OpportunityBuilder AddProvisionGapItem()
        {
            AddOpportunityItem(OpportunityType.ProvisionGap);

            return this;
        }

        public Domain.Models.Opportunity Build() => _opportunity;
    }
}
