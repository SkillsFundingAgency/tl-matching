using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderProximitySearchParamViewModel
    {
        [Required(ErrorMessage = "You must enter a postcode")]
        public string Postcode { get; set; }
    }
}