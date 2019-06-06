using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class QualificationSearchResultViewModel
    {
        public int QualificationId { get; set; }
        public string LarId { get; set; }
        public string ShortTitle { get; set; }
        public string Title { get; set; }
        public IList<int> RouteIds { get; set; }
        public IList<RouteViewModel> Routes { get; set; }
    }
}