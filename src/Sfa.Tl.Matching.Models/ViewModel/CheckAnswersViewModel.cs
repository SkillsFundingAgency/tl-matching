using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class CheckAnswersViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string EmployerName { get; set; }
        public string RouteName { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int RouteId { get; set; }
        public string JobRole { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public List<ReferralsViewModel> Providers { get; set; }
        public NavigationViewModel Navigation { get; set; }
    }
}