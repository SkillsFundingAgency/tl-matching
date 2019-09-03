using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class PlacementInformationSaveViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }

        [MinLength(2, ErrorMessage = "You must enter a job role using 2 or more characters")]
        [MaxLength(100, ErrorMessage = "You must enter a job role that is 100 characters or fewer")]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "You must enter a job role using letters")]
        public string JobRole { get; set; }

        [Required(ErrorMessage = "You must tell us whether the employer knows how many students they want for this job at this location")]
        public bool? PlacementsKnown { get; set; }

        public int? Placements { get; set; }

        public bool NoSuitableStudent { get; set; }
        public bool HadBadExperience { get; set; }
        public bool ProvidersTooFarAway { get; set; }

        public int RouteId { get; set; }
        public string Postcode { get; set; }
        public short SearchRadius { get; set; }
        public int SearchResultProviderCount { get; set; }

        public OpportunityType OpportunityType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNameAka { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(CompanyNameAka) ?
            $"{CompanyName} ({CompanyNameAka})" : CompanyName;
    }
}