using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderFeedbackDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int ProviderId { get; set; }
        public string Companyname { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public DateTime? ProviderFeedbackEmailSentOn { get; set; }
        
    }
}
