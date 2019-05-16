using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchParametersViewModel
    {
        [Required(ErrorMessage = "You must enter a UKPRN")]
        [DisplayName("UKPRN")]
        public long? UkPrn { get; set; }

        public bool ShowAllProvider { get; set; }
    }
}