using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderNameViewModel
    {
        public string DisplayName { get; set; }
        public string VenueName { get; set; }
        public string Postcode { get; set; }
        public string ProviderName => 
            ProviderDisplayExtensions.GetDisplayText(VenueName, Postcode, DisplayName, false);
    }
}