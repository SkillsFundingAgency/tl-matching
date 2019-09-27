using System.Net.Http;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class GoogleDistanceMatrixHttpClientFactory : HttpClientFactory
    {
        public HttpClient Get(decimal originLatitude = 52.400997M, decimal originLongitude = -1.508122M, string destinationPolyline = "enc:chjyHb%7DW:", string responseTown = "Coventry")
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

            var uri =
                $"https://example.com/distancematrix/json?units=imperial&origins={originLatitude}%2C{originLongitude}&mode=driving&destinations={destinationPolyline}&key=TEST_KEY";

            return CreateClient(response, uri);
        }
    }
}