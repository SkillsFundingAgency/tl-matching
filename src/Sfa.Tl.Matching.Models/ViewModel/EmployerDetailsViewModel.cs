using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerDetailsViewModel
    {
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "You must enter a contact name for placements")]
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}