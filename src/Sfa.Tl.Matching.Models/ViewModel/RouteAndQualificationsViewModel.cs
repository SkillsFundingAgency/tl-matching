using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class RouteAndQualificationsViewModel
    {
        public string RouteName { get; set; }
        public IEnumerable<string> QualificationShortTitles { get; set; }
    }
}