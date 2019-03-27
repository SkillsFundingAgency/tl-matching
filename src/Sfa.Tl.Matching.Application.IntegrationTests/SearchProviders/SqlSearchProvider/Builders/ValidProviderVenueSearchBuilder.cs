using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders
{
    public class ValidProviderVenueSearchBuilder
    {
        public Domain.Models.ProviderVenue Build()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            var location = geometryFactory.CreatePoint(new Coordinate(double.Parse("-1.508122"), double.Parse("52.400997")));

            return new Domain.Models.ProviderVenue
            {
                Provider = new Domain.Models.Provider
                {
                    UkPrn = 10203040,
                    Name = "SQL Search Provider",
                    Status = true,
                    PrimaryContact = "Test",
                    PrimaryContactEmail = "Test@test.com",
                    PrimaryContactPhone = "0123456789",
                    SecondaryContact = "Test 2",
                    SecondaryContactEmail = "Test2@test.com",
                    SecondaryContactPhone = "0123456789",
                    Source = "Test"
                },
                Postcode = "CV1 2WT",
                Latitude = 52.400997m,
                Longitude = -1.508122m,
                Location = location,
                Source = "Test",
                ProviderQualification = new List<Domain.Models.ProviderQualification>
                {
                    new Domain.Models.ProviderQualification
                    {
                        Qualification = new Qualification
                        {
                            LarsId = "12345678",
                            Title = "Qualification Title",
                            ShortTitle = "Short Title",
                            QualificationRoutePathMapping = new List<Domain.Models.QualificationRoutePathMapping>
                            {
                                new Domain.Models.QualificationRoutePathMapping
                                {
                                    PathId = 16,
                                    Source = "Test"
                                }
                            }
                        },
                        Source = "Test"
                    }
                }
            };
        }
    }
}