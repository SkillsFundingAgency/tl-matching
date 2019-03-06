using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Sfa.Tl.Matching.Application.Services
{
    public class LocationService : ILocationService
    {
        public bool IsValidPostCode(string postCode)
        {
            throw new System.NotImplementedException();
        }

        public LocationDto GetGeoLocationData(string postCode)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ILocationService
    {
        bool IsValidPostCode(string postCode);
        LocationDto GetGeoLocationData(string postCode);
    }

    public class LocationDto
    {
        public string PostCode { get; set; }
        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }

        public string OutCode { get; set; }

        public string Admin_District { get; set; }

        public string Admin_County { get; set; }

        public Codes Codes { get; set; }
    }

    public class Codes
    {
        public string Admin_District { get; set; }

        public string Admin_County { get; set; }

        public string Admin_Ward { get; set; }

        public string Parish { get; set; }

        public string Parliamentary_Constituency { get; set; }

        public string Ccg { get; set; }

        public string Ced { get; set; }

        public string Nuts { get; set; }
    }

}