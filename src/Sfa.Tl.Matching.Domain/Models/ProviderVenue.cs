{
    public class ProviderVenue : BaseEntity
    {
        public int ProviderId { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Source { get; set; }

        public virtual Provider Provider { get; set; }
    }
}
