namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ReferralsViewModel
    {
        public string Name { get; set; }
        public string ProviderDisplayName { get; set; }

        public string ProviderVenueName { get; set; }
        public string Postcode { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public string DisplayText => ProviderVenueName == Postcode
            ? ProviderDisplayName
            : $"{ProviderVenueName} (part of {ProviderDisplayName})";
    }
}