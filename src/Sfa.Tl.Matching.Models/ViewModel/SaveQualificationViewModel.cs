using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class SaveQualificationViewModel
    {
        public int QualificationId { get; set; }
        [Required(ErrorMessage = "You must enter a short title")]
        public string ShortTitle { get; set; }
        public string Title { get; set; }
        public string Source { get; set; }
        public IList<RouteViewModel> Routes { get; set; }
        public string SearchString { get; set; }
    }
}