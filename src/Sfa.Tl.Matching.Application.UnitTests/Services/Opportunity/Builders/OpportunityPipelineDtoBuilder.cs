﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityPipelineDtoBuilder
    {
        private readonly OpportunityPipelineDto _dto;

        public OpportunityPipelineDtoBuilder()
        {
            _dto = new OpportunityPipelineDto
            {
                CompanyName = "CompanyName",
                ReferralItems = new List<ReferralItemDto>(),
                ProvisionGapItems = new List<ProvisionGapItemDto>()
            };
        }

        internal OpportunityPipelineDtoBuilder AddReferralItem()
        {
            _dto.ReferralItems.Add(new ReferralItemDto
            {
                Workplace = "London SW1 1AA",
                JobRole = "Referral",
                PlacementsKnown = true,
                Placements = 5,
                ProviderName = "Provider",
                Town = "London",
                Postcode = "SW1 1AB",
                ProviderVenueTownAndPostcode = "London SW1 1AB",
                DistanceFromEmployer = 1.5M
            });

            return this;
        }

        internal OpportunityPipelineDtoBuilder AddProvisionGapItem()
        {
            _dto.ProvisionGapItems.Add(new ProvisionGapItemDto
            {
                Workplace = "London SW1 1AA",
                JobRole = "ProvisionGap",
                Reason = "Reason",
                PlacementsKnown = true,
                Placements = 5
            });

            return this;
        }

        public OpportunityPipelineDto Build() => _dto;
    }
}