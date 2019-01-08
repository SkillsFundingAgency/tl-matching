﻿using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class LocalAuthorityMapping
    {
        public Guid Id { get; set; }
        public string LocalAuthorityCode { get; set; }
        public string LocalAuthority { get; set; }
        public string LocalEnterprisePartnership { get; set; }
        public bool InMultipleLeps { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Address> Address { get; set; }
    }
}
