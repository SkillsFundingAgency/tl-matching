using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public Guid EntityRefId { get; set; }
        public Guid? LocalAuthorityMappingId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Employer Employer { get; set; }
        public virtual Provider Provider { get; set; }
        public virtual LocalAuthorityMapping LocalAuthorityMapping { get; set; }
        public virtual ICollection<IndustryPlacement> IndustryPlacement { get; set; }
        public virtual ICollection<ProviderCourses> ProviderCourses { get; set; }
    }
}
