using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityItemBuilder
    {
        private readonly OpportunityItem _opportunityItem;

        public OpportunityItemBuilder()
        {
            _opportunityItem = new OpportunityItem
            {
                Id = 1,
                OpportunityId = 2,
                OpportunityType = OpportunityType.Referral.ToString(),
                CreatedBy = "CreatedBy",
                Opportunity = new Domain.Models.Opportunity
                {
                    Id = 2,
                    CreatedBy = "CreatedBy"
                }
            };
        }

        public OpportunityItemBuilder AddEmployer()
        {
            _opportunityItem.Opportunity.EmployerCrmId = new Guid("33333333-3333-3333-3333-333333333333");
            _opportunityItem.Opportunity.Employer = new Domain.Models.Employer
            {
                Id = 3,
                CompanyName = "CompanyName",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            };

            return this;
        }

        public OpportunityItemBuilder AddProvisionGap()
        {
            _opportunityItem.ProvisionGap ??= new List<ProvisionGap>();
            _opportunityItem.OpportunityType = OpportunityType.ProvisionGap.ToString();
            _opportunityItem.ProvisionGap.Add(
                new ProvisionGap
                {
                    Id = 4,
                    OpportunityItemId = 2,
                    NoSuitableStudent = true,
                    HadBadExperience = true,
                    ProvidersTooFarAway = true,
                    CreatedBy = "CreatedBy"
                });

            return this;
        }

        public OpportunityItem Build() => _opportunityItem;
    }
}
