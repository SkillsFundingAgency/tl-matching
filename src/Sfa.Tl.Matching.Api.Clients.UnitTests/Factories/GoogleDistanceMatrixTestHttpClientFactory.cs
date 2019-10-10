using System.Net;
using System.Net.Http;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class GoogleDistanceMatrixTestHttpClientFactory : TestHttpClientFactory
    {
        public HttpClient Get(string originPostcode = "CV1 2WT",
            string destinationPostcode = "SW1A 2AA",
            long arrivalTimeSeconds = 1570014314)
        {
            var response = new GoogleJourneyTimeResponse
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
                $"https://example.com/distancematrix/json?units=imperial&origins={origin}&mode=driving&arrival_time={arrivalTimeSeconds}&destinations={destination}&key=TEST_KEY";

            return CreateClient(response, uri);
        }
    }
}