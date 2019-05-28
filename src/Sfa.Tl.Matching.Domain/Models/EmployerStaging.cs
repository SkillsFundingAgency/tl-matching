using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class EmployerStaging : BaseEntity
    {
        [MergeKey]
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

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ChecksumCol { get; set; }
    }
}