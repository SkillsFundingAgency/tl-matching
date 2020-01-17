using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileWriter.ProviderProximityReport.Builders
{
    internal class ProviderProximityReportDtoBuilder
    {
        private readonly ProviderProximityReportDto _dto;

        public ProviderProximityReportDtoBuilder()
        {
            _dto = new ProviderProximityReportDto
            {
                Postcode = "CV1 2WT",
                SkillAreas = new List<string>(),
                Providers = new List<ProviderProximityReportItemDto>()
            };
        }

        internal ProviderProximityReportDtoBuilder AddProvider()
        {
            _dto.Providers.Add(new ProviderProximityReportItemDto
            {
                ProviderVenueTown = "Coventry",
                ProviderVenuePostcode = "CV1 5FB",
                Distance = 3.5,
                ProviderName = "Provider",
                ProviderDisplayName = "Provider Display Name",
                ProviderVenueName = "Coventry Cathedral",
                PrimaryContact = "Primary contact",
                PrimaryContactEmail = "Primary contact email",
                PrimaryContactPhone = "Primary contact telephone",
                SecondaryContact = "Secondary contact",
                SecondaryContactEmail = "Secondary contact email",
                SecondaryContactPhone = "Secondary contact telephone",
                Routes = new List<RouteAndQualificationsDto>
                {
                    new RouteAndQualificationsDto
                    {
                        RouteId = 6,
                        RouteName = "Digital",
                        QualificationShortTitles = new List<string>
                        {
                            ".NET for Dummies"
                        }
                    }
                }
            });

            return this;
        }

        internal ProviderProximityReportDtoBuilder AddSkillAreas()
        {
            _dto.SkillAreas.Add("Creative and design");
            _dto.SkillAreas.Add("Digital");
            _dto.SkillAreas.Add("Health and science");

            return this;
        }

        public ProviderProximityReportDto Build() => _dto;
    }
}
