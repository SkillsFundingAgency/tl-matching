using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders
{
    internal class OpportunityReportDtoBuilder
    {
        private readonly OpportunityReportDto _dto;

        public OpportunityReportDtoBuilder()
        {
            _dto = new OpportunityReportDto
            {
                CompanyName = "Company Name",
                ReferralItems = new List<ReferralItemDto>(),
                ProvisionGapItems = new List<ProvisionGapItemDto>()
            };
        }

        internal OpportunityReportDtoBuilder AddReferralItem()
        {
            _dto.ReferralItems.Add(new ReferralItemDto
            {
                Workplace = "London SW1 1AA",
                JobRole = "Referral",
                PlacementsKnown = true,
                Placements = 5,
                ProviderDisplayName = "Provider",
                Town = "London",
                Postcode = "SW1 1AB",
                ProviderVenueName = "ProviderVenueName",
                ProviderVenueTownAndPostcode = "London SW1 1AB",
                DistanceFromEmployer = 1.5M
            });

            return this;
        }

        internal OpportunityReportDtoBuilder AddProvisionGapItem()
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

        public OpportunityReportDto Build() => _dto;
    }
}
