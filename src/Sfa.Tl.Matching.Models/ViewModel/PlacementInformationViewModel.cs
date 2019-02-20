using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.ValidationAttributes;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class PlacementInformationViewModel
    {
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "You must tell us what specific job the placement student would do")]
        [MinLength(2, ErrorMessage = "You must enter a job role using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a job role using 99 characters or less")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "You must tell us whether the employer knows how many placements they want at this location")]
        public bool PlacementsKnown { get; set; }

        [Required(ErrorMessage = "You must estimate how many placements the employer wants at this location")]
        [Min(1, ErrorMessage = "You must enter a number that is 1 or more")]
        [Max(999, ErrorMessage = "You must enter a number that is 999 or less")]
        public short Placements { get; set; }
    }
}