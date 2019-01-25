using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Employer
    {
        public int Id { get; set; }
        public Guid CrmId { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public int AupaStatus { get; set; }
        public int CompanyType { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string AddressName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Owner { get; set; }
    }
}