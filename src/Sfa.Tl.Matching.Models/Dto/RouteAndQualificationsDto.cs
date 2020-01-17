using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class RouteAndQualificationsDto
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}