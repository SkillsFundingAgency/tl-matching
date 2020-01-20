using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityItemBuilder
    {
        private readonly OpportunityItem _opportunityItem;

        public OpportunityItemBuilder(int id = 1, int opportunityId = 2)
        {
            _opportunityItem = new OpportunityItem
            {
                Id = id,
                OpportunityId = 2,
                Postcode = "Postcode",
                RouteId = 1,
                Placements = 1,
                OpportunityType = OpportunityType.Referral.ToString(),
                CreatedBy = "CreatedBy",
                Opportunity = new Domain.Models.Opportunity
                {
                    Id = opportunityId,
                    CreatedBy = "CreatedBy"
                }
            };
        }

        public OpportunityItemBuilder AddEmployer(int id = 3)
        {
            var crmId = Guid.NewGuid();
            _opportunityItem.Opportunity.EmployerCrmId = crmId;
            _opportunityItem.Opportunity.Employer = new Domain.Models.Employer
            {
                Id = id,
                CrmId = crmId,
                CompanyName = "CompanyName",
                AlsoKnownAs = "AlsoKnownAs",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy",
                Owner = "Owner"
            };

            return this;
        }

        public OpportunityItemBuilder AddProvisionGap(int id = 4, bool noSuitableStudent = true, bool hadBadExperience = true, bool providersTooFarAway = true)
        {
            if (_opportunityItem.ProvisionGap == null)
            {
                _opportunityItem.ProvisionGap = new List<ProvisionGap>();
            }

            _opportunityItem.OpportunityType = OpportunityType.ProvisionGap.ToString();
            _opportunityItem.ProvisionGap.Add(
                new ProvisionGap
                {
                    Id = id,
                    OpportunityItemId = 2,
                    NoSuitableStudent = noSuitableStudent,
                    HadBadExperience = hadBadExperience,
                    ProvidersTooFarAway = providersTooFarAway,
                    CreatedBy = "CreatedBy"
                });

            return this;
        }

        public OpportunityItem Build() => _opportunityItem;
    }
}
