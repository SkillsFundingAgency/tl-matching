using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderDetailViewModel
    {
        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }

        public bool IsEnabledForReferral { get; set; }
        public bool IsEnabledForSearch { get; set; }

        public List<ProviderVenueViewModel> ProviderVenues { get; set; }
    }
}