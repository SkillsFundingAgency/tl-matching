namespace Sfa.Tl.Matching.Models.Dto
{
    public class ProviderQualificationDto
    {
        public int ProviderVenueId { get; set; }
        public int QualificationId { get; set; }
        public int? NumberOfPlacements { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}