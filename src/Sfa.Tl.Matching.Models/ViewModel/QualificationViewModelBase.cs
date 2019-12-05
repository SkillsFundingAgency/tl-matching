using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class QualificationViewModelBase
    {
        public int QualificationId { get; set; }
        public string LarId { get; set; }
        [Required(ErrorMessage = "You must enter a short title")]
        public string ShortTitle { get; set; }
        public string Title { get; set; }
        public IList<RouteSummaryViewModel> Routes { get; set; }
    }
}