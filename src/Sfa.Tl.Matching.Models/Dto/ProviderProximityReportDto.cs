using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderProximityReportDto
    {
        public string Postcode { get; set; }

        public IList<ProviderProximityReportItemDto> Providers{ get; set; }
        public IList<string> SkillAreas { get; set; } = new List<string>();
    }
}