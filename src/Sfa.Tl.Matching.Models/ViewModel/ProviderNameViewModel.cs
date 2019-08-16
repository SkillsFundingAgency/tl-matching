namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderNameViewModel
    {
        public string DisplayName { get; set; }
        public string VenueName { get; set; }
        public string Postcode { get; set; }
        public string ProviderName => VenueName == Postcode ? DisplayName : $"{VenueName} (part of {DisplayName})";
    }
}