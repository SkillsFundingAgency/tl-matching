﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderVenue : BaseEntity
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(9, 6)")]
        public decimal? Longitude { get; set; }
        public bool IsRemoved { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public string Source { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<ProviderQualification> ProviderQualification { get; set; }
        public virtual ICollection<Referral> Referral { get; set; }

        public Point Location { get; set; }
    }
}