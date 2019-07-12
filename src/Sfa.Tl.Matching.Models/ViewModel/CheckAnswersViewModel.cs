using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameAka { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(CompanyNameAka) ?
            $"{CompanyName} ({CompanyNameAka})" : CompanyName;
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int RouteId { get; set; }
        public string JobRole { get; set; }
        public int? Placements { get; set; }
        public bool? PlacementsKnown { get; set; }
        public List<ReferralsViewModel> Providers { get; set; }
        public string PlacementsDetail => PlacementsKnown.GetValueOrDefault() ? Placements.ToString() : "at least 1";
    }
}