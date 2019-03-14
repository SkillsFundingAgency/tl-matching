// ReSharper disable InconsistentNaming
namespace Sfa.Tl.Matching.Models.Dto
{
    public class PostCodeLookupResultDto
    {
        public string PostCode { get; set; }
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string OutCode { get; set; }

        public string Admin_District { get; set; }

        public string Admin_County { get; set; }

        public LocationCodesDto Codes { get; set; }
    }
}