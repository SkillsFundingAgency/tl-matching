using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class EmployerConsentViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int OpportunityItemCount { get; set; }
        public string EmployerName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactEmail { get; set; }
        public string EmployerContactPhone { get; set; }
        public string CompanyName { get; set; }
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm that we can share the employer’s details with the selected providers")]
        public bool ConfirmationSelected { get; set; }
    }
}