
namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderSearchResultItemViewModel
    {
        public int ProviderId { get; set; }
        public long? UkPrn { get; set; }
        public string ProviderName { get; set; }
        public bool IsFundedForNextYear { get; set; }
    }
}