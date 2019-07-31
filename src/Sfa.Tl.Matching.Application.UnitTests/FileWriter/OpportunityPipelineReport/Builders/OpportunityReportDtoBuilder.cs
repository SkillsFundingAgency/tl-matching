using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileWriter.OpportunityPipelineReport.Builders
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
                JobRole = "Referral Role",
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

        internal OpportunityReportDtoBuilder AddProvisionGapItem()
        {
            _dto.ProvisionGapItems.Add(new ProvisionGapItemDto
            {
                Workplace = "London SW1 1AA",
                JobRole = "Provision Gap Role",
                Reason = "Reason",
                PlacementsKnown = false,
                Placements = null
            });

            return this;
        }

        public OpportunityReportDto Build() => _dto;
    }
}
