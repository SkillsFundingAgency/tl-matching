using System;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public Guid EntityRefId { get; set; }
        public int ContactTypeId { get; set; }
        public int PreferredContactMethodType { get; set; }
        public bool IsPrimary { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string BusinessPhone { get; set; }
        public string MobilePhone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Employer Employer { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
