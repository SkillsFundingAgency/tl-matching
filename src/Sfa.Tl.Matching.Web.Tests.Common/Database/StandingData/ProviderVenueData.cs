﻿using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite;
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

            return new List<ProviderVenue>
            {
                new ProviderVenue
                {
                    Id = 1,
                    Name = "Venue 1 Name",
                    Provider = BuildProvider(true),
                    Postcode = "CV1 2WT",
                    Town = "Coventry",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                },
                new ProviderVenue
                {
                    Id = 2,
                    Name = "Venue 2 Name",
                    Provider = BuildProvider(true),
                    Postcode = "CV1 1EE",
                    Town = "Coventry",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                }
            };
        }

        private static IPoint CreatePointLocation(double latitude, double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
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
                SecondaryContactPhone = "0123456789",
                IsCdfProvider = isCdfProvider,
                IsEnabledForReferral = true,
                IsTLevelProvider = true,
                Source = "Test"
            };
        }

        private static ICollection<ProviderQualification> BuildProviderQualifications()
        {
            return new List<ProviderQualification>
            {
                new ProviderQualification
                {
                    Qualification = new Qualification
                    {
                        LarsId = "12345678",
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
                    },
                    Source = "Test"
                }
            };
        }
    }
}