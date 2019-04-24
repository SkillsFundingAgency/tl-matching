namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class HideProviderViewModel
    {
        public int ProviderId { get; set; }
        public long UkPrn { get; set; }
        public string ProviderName { get; set; }
        public bool IsEnabledForSearch { get; set; }
    }
}