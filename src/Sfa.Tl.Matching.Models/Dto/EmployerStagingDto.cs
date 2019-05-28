using System;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class EmployerStagingDto
    {
        public Guid CrmId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public string CompanyNameSearch { get; set; }
        public string Aupa { get; set; }
        public string CompanyType { get; set; }
        public string PrimaryContact { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string Owner { get; set; }
        public string CreatedBy { get; set; }
        public string CompanyNameWithAka => !string.IsNullOrWhiteSpace(AlsoKnownAs) ? 
            $"{CompanyName} ({AlsoKnownAs})" : CompanyName;
    }
}