using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderProximityReportDto
    {
        public string Postcode { get; set; }

        public IList<ProviderProximityReportItemDto> Providers{ get; set; }
        public IList<string> SkillAreas { get; set; }
    }
}