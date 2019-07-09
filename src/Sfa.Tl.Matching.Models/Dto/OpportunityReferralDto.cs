namespace Sfa.Tl.Matching.Models.Dto
{
    public class OpportunityReferralDto
    {
        public int OpportunityId { get; set; }
        public int ReferralId { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPrimaryContact { get; set; }
        public string ProviderPrimaryContactEmail { get; set; }
        public string ProviderSecondaryContactEmail { get; set; }
        public string ProviderVenuePostcode { get; set; }
        public string RouteName { get; set; }
        public short SearchRadius { get; set; }
        public string JobRole { get; set; }
        public string Postcode { get; set; }
        public string CompanyName { get; set; }
        public string EmployerContact { get; set; }
        public string EmployerContactPhone { get; set; }
        public string EmployerContactEmail { get; set; }
        public bool? PlacementsKnown { get; set; }
        public int? Placements { get; set; }
        public string CreatedBy { get; set; }
    }
}
