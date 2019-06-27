using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerDetailsViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string EmployerName { get; set; }

        [Required(ErrorMessage = "You must enter a contact name for placements")]
        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a contact name using 99 characters or less")]
        [RegularExpression(@"^[a-zA-Z0-9'\s-]*$", ErrorMessage = "You must enter a contact name using only letters, hyphens and apostrophes")]
        public string EmployerContact { get; set; }

        [Required(ErrorMessage = "You must enter a contact email for placements")]
        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "You must enter a valid email")]
        public string EmployerContactEmail { get; set; }

        [Required(ErrorMessage = "You must enter a contact telephone number for placements")]
        public string EmployerContactPhone { get; set; }
    }
}