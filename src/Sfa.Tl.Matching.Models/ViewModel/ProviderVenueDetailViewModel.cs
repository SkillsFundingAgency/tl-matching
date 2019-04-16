using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderVenueDetailViewModel
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public long UkPrn { get; set; }
        public string Postcode { get; set; }
        [MaxLength(400, ErrorMessage = "You must enter a venue name that is 400 characters or fewer")]
        public string VenueName { get; set; }
        public string Source { get; set; }
        public List<QualificationDetailViewModel> Qualifications { get; set; }
    }
}