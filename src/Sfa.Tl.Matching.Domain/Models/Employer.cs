using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Employer
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string AlsoKnownAs { get; set; }
        public int CompanyType { get; set; }
        public int AupaStatus { get; set; }
        public string Website { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<NotificationHistory> NotificationHistory { get; set; }
    }
}
