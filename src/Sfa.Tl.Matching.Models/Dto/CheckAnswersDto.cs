using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class CheckAnswersDto
    {
        public int OpportunityId { get; set; }
        public bool? ConfirmationSelected { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}