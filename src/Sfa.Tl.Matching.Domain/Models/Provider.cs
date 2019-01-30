using System;
using System.Collections.Generic;

// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Domain.Models
{
    public class Provider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ukprn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual ICollection<NotificationHistory> NotificationHistory { get; set; }
        public virtual ICollection<ProviderCourses> ProviderCourses { get; set; }
    }
}