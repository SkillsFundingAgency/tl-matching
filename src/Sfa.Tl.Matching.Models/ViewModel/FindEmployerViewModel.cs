using System;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class FindEmployerViewModel
    {
        public int OpportunityId { get; set; }
        public int OpportunityItemId { get; set; }
        public Guid SelectedEmployerCrmId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string PreviousCompanyName { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(AlsoKnownAs) ?
            $"{CompanyName} ({AlsoKnownAs})" : CompanyName;
    }
}