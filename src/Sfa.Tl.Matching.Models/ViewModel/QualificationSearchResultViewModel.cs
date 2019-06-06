using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class QualificationSearchResultViewModel
    {
        public int QualificationId { get; set; }
        public string LarId { get; set; }
        [MaxLength(99, ErrorMessage = "You must enter a short title that is 100 characters or fewer")]
        public string ShortTitle { get; set; }
        public string Title { get; set; }
        public IList<int> RouteIds { get; set; }
        public IList<RouteViewModel> Routes { get; set; }
    }
}