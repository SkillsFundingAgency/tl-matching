using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderVenueAddViewModel
    {
        public int ProviderId { get; set; }
        public long UkPrn { get; set; }
        [Required(ErrorMessage = "You must enter a postcode")]
        public string Postcode { get; set; }
        public string Source { get; set; } = string.Empty;
    }
}