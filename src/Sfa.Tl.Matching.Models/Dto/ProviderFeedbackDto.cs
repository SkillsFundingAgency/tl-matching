using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderFeedbackDto
    {
        public string ProviderName { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string EmployerCompanyName { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public IEnumerable<string> Routes { get; set; }
    }
}