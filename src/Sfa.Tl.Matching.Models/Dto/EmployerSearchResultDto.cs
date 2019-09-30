using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerSearchResultDto
    {
        public Guid CrmId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
    }
}