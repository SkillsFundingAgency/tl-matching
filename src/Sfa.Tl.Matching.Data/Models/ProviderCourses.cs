﻿using System;

namespace Sfa.Tl.Matching.Data.Models
{
    public class ProviderCourses
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid CourseId { get; set; }
        public Guid AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual Course Course { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
