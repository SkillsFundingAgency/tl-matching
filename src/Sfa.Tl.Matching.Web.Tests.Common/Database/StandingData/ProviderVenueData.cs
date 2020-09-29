using System.Collections.Generic;
using System.Linq;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData
{
    internal class ProviderVenueData
    {
        private const decimal Latitude = 52.400997m;
        private const decimal Longitude = -1.508122m;

        internal static IList<ProviderVenue> Create()
        {
            var location = CreatePointLocation((double)Latitude, (double)Longitude);

            var provider = BuildProvider(true);
            var qualifications = BuildQualifications();

            return new List<ProviderVenue>
            {
                new ProviderVenue
                {
                    Id = 1,
                    Name = "Venue 1 Name",
                    Provider = provider,
                    Postcode = "CV1 2WT",
                    Town = "Coventry",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications(qualifications.First())
                },
                new ProviderVenue
                {
                    Id = 2,
                    Name = "Venue 2 Name",
                    Provider = provider,
                    Postcode = "CV1 1EE",
                    Town = "Coventry",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications(qualifications.First())
                }
            };
        }

        private static Provider BuildProvider(bool isCdfProvider)
        {
            return new Provider
            {
                Id = 1,
                UkPrn = 10203040,
                Name = "SQL Search Provider",
                DisplayName = "SQL Search Provider Display Name",
                PrimaryContact = "Test",
                PrimaryContactEmail = "Test@test.com",
                PrimaryContactPhone = "0123456789",
                SecondaryContact = "Test 2",
                SecondaryContactEmail = "Test2@test.com",
                SecondaryContactPhone = "0987654321",
                IsCdfProvider = isCdfProvider,
                IsEnabledForReferral = true,
                IsTLevelProvider = true,
                Source = "Test"
            };
        }

        private static ICollection<Qualification> BuildQualifications()
        {
            return new List<Qualification>
            {
                new Qualification
                {
                    Id = 1,
                    LarId = "12345678",
                    Title = "Qualification Title",
                    ShortTitle = "Short Title",
                    QualificationRouteMapping = new List<QualificationRouteMapping>
                    {
                        new QualificationRouteMapping
                        {
                            RouteId = 1,
                            Source = "Test"
                        }
                    }
                }
            };
        }

        private static ICollection<ProviderQualification> BuildProviderQualifications(Qualification qualification)
        {
            return new List<ProviderQualification>
            {
                new ProviderQualification
                {
                    Qualification = qualification,
                    Source = "Test"
                }
            };
        }

        private static Point CreatePointLocation(double latitude, double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        }
    }
}