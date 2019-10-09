using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderFeedbackDto
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public int ProviderId { get; set; }
        public string Companyname { get; set; }
        public string ProviderPrimaryContactName { get; set; }
        public string ProviderPrimaryContactEmail { get; set; }
        public string ProviderSecondaryContactName { get; set; }
        public string ProviderSecondaryContactEmail { get; set; }
        public DateTime? ProviderFeedbackEmailSentOn { get; set; }
        
    }
}
