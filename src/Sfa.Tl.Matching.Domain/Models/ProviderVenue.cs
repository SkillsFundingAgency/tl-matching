using System.Collections.Generic;
using GeoAPI.Geometries;

namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderVenue : BaseEntity
    {
        public int ProviderId { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Source { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public bool IsEnabledForSearch { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual ICollection<ProviderQualification> ProviderQualification { get; set; }
        public virtual ICollection<Referral> Referral { get; set; }

        public IPoint Location { get; set; }
    }
}