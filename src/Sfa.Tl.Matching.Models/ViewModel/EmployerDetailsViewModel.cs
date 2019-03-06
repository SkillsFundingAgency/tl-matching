using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerDetailsViewModel
    {
        public int OpportunityId { get; set; }
        public string EmployerName { get; set; }

        [Required(ErrorMessage = "You must enter a contact name for placements")]
        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a contact name using 99 characters or less")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "You must enter a contact name using letters only")]
        public string Contact { get; set; }

        [Required(ErrorMessage = "You must enter a contact email for placements")]
        [EmailAddress(ErrorMessage = "You must enter a valid email")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "You must enter a contact telephone number for placements")]
        public string ContactPhone { get; set; }
    }
}