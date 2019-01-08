using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class Provider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ukprn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Contact> Contact { get; set; }
        public virtual ICollection<NotificationHistory> NotificationHistory { get; set; }
        public virtual ICollection<ProviderCourses> ProviderCourses { get; set; }
    }
}
