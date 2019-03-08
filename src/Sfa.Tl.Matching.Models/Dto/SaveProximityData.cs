namespace Sfa.Tl.Matching.Models.Dto
{
    public class SaveProximityData
    {
        public long UkPrn { get; set; }
        public string PostCode { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}