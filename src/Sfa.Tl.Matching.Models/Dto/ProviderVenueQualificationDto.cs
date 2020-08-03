using System.Collections;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderVenueQualificationDto
    {
        public long UkPrn { get; set; }
        public bool InMatchingService { get; set; }
        public string ProviderName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsCdfProvider { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string VenuePostcode { get; set; }
        public string Town { get; set; }
        public string VenueName { get; set; }
        public bool VenueIsEnabledForReferral { get; set; }
        public bool VenueIsRemoved { get; set; }
        public string LarId { get; set; }
        public string QualificationTitle { get; set; }
        public string QualificationShortTitle { get; set; }
        public bool QualificationIsOffered { get; set; }
        public IList<string> Routes { get; set; }
    }
}