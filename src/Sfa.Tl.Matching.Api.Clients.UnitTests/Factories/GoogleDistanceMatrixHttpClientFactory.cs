using System.Net;
using System.Net.Http;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class GoogleDistanceMatrixHttpClientFactory : HttpClientFactory
    {
        public HttpClient Get(string originPostcode = "CV1 2WT",
            decimal originLatitude = 52.400997M, 
            decimal originLongitude = -1.508122M, 
            string destinationPostcode = "SW1A 2AA",
            string destinationPolyline = "enc:chjyHb%7DW:", 
            long arrivalTimeSeconds = 1570014314)
        {
            var response = new GoogleDistanceMatrixResponse
            {
                DestinationAddresses = new[]
                {
                    "10 Downing St, Westminster, London SW1A 2AA, UK" 
                },
                OriginAddresses = new[]
                {
                    "Quinton Rd, Coventry CV1 2WT, UK"
                },
                Rows = new []
                {
                    new Row
                    {
                        Elements = new []
                        {
                            new Element
                            {
                                Distance = new Distance
                                {
                                    Text = "107 mi",
                                    Value =  172648
                                },
                                Duration = new Duration
                                {
                                    Text = "2 hours 7 mins",
                                    Value = 7603
                                },
                                Status = "OK"
                            }
                        }
                    } 
                },
                Status = "OK"
            };

            var origin = WebUtility.UrlEncode(originPostcode);
            var destination = WebUtility.UrlEncode(destinationPostcode);

            var uri =
                //$"https://example.com/distancematrix/json?units=imperial&origins={originLatitude}%2C{originLongitude}&mode=driving&arrival_time={arrivalTimeSeconds}&destinations={destinationPolyline}&key=TEST_KEY";
                $"https://example.com/distancematrix/json?units=imperial&origins={origin}&mode=driving&arrival_time={arrivalTimeSeconds}&destinations={destination}&key=TEST_KEY";

            return CreateClient(response, uri);
        }
    }
}