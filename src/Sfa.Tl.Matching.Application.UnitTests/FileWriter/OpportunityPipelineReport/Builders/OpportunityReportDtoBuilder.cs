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
                ProviderDisplayName = "Provider",
                Town = "London",
                Postcode = "SW1 1AB",
                ProviderVenueName = "ProviderVenueName",
                ProviderVenueTownAndPostcode = "London SW1 1AB",
                DistanceFromEmployer = 1.5M,
                PrimaryContact = "Primary contact",
                PrimaryContactEmail = "Primary contact email",
                PrimaryContactPhone = "Primary contact telephone",
                SecondaryContact = "Secondary contact",
                SecondaryContactEmail = "Secondary contact email",
                SecondaryContactPhone = "Secondary contact telephone"
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
