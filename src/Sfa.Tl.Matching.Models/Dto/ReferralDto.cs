namespace Sfa.Tl.Matching.Models.Dto
{
    public class ReferralDto
    {
        public int ProviderVenueId { get; set; }
        public decimal DistanceFromEmployer { get; set; }
        public int? JourneyTimeByCar{ get; set; }
        public int? JourneyTimeByPublicTransport { get; set; }
        public string Name { get; set; }
        public string Postcode { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderVenueName { get; set; }
        public string CreatedBy { get; set; }
    }
}