using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderDetailViewModel
    {
        public ProviderDetailViewModel()
        {
            ProviderVenues = new List<ProviderVenueViewModel>();
        }

        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "You must tell us how the provider name should be displayed")]
        [MaxLength(400, ErrorMessage = "You must enter a provider name that is 400 characters or fewer")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "You must tell us who the primary contact is for industry placements")]
        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(100, ErrorMessage = "You must enter a contact name that is 100 characters or fewer")]
        [RegularExpression(@"^(?!^\d+$)^.+$", ErrorMessage = "You must enter a contact name using letters")]
        public string PrimaryContact { get; set; }

        [Required(ErrorMessage = "You must enter an email for the primary contact")]
        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string PrimaryContactEmail { get; set; }
        
        [PhoneNumber(FieldName = "primary contact", IsRequired = true)]
        public string PrimaryContactPhone { get; set; }

        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(100, ErrorMessage = "You must enter a contact name that is 100 characters or fewer")]
        [RegularExpression(@"^(?!^\d+$)^.+$", ErrorMessage = "You must enter a contact name using letters")]
        public string SecondaryContact { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string SecondaryContactEmail { get; set; }

        [PhoneNumber(FieldName = "primary contact", IsRequired = false)]
        public string SecondaryContactPhone { get; set; }

        [Required(ErrorMessage = "You must tell us whether the provider should receive referrals")]
        public bool? IsEnabledForReferral { get; set; } = true;

        public string SubmitAction { get; set; }
        public string Source { get; set; }
        public bool IsCdfProvider { get; set; } = true;
        public bool IsTLevelProvider { get; set; } = false;
        
        public IList<ProviderVenueViewModel> ProviderVenues { get; set; }

        public bool IsSaveSection=>
            !string.IsNullOrWhiteSpace(SubmitAction)
            && string.Equals(SubmitAction, "SaveSection", StringComparison.InvariantCultureIgnoreCase);

        public bool IsSaveAndAddVenue =>
            !string.IsNullOrWhiteSpace(SubmitAction)
            && string.Equals(SubmitAction, "SaveAndAddVenue", StringComparison.InvariantCultureIgnoreCase);
    }
}