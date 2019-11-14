using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerDetailsViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(AlsoKnownAs) ?
            $"{CompanyName} ({AlsoKnownAs})" : CompanyName;

        [Required(ErrorMessage = "You must enter a contact name for placements")]
        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(100, ErrorMessage = "You must enter a contact name that is 100 characters or fewer")]
        [RegularExpression(@"^[a-zA-Z'\s-]*$", ErrorMessage = "You must enter a contact name using only letters, hyphens and apostrophes")]
        public string PrimaryContact { get; set; }

        [Required(ErrorMessage = "You must enter a contact email for placements")]
        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "You must enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a contact telephone number for placements")]
        public string Phone { get; set; }
    }
}