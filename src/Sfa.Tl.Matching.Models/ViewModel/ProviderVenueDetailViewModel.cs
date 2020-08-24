using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderVenueDetailViewModel
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string ProviderName { get; set; }
        public string Postcode { get; set; }
        [Required(ErrorMessage = "You must tell us how the venue name should be displayed")]
        [MaxLength(400, ErrorMessage = "You must enter a venue name that is 400 characters or fewer")]
        public string Name { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public string Source { get; set; }
        public bool IsFromAddVenue { get; set; }
        public string SubmitAction { get; set; }
        public bool IsRemoved { get; set; }

        public List<QualificationDetailViewModel> Qualifications { get; set; }

        public ProviderVenueDetailViewModel()
        {
            IsRemoved = false;
        }
    }
}