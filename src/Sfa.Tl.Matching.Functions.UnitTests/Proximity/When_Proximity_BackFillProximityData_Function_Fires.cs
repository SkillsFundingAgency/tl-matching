using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_Proximity_BackFillProximityData_Function_Fires
    {
        private readonly ILocationApiClient _locationApiClient;
        private readonly IRepository<ProviderVenue> _providerVenueRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_Proximity_BackFillProximityData_Function_Fires()
        {
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _providerVenueRepository = Substitute.For<IRepository<ProviderVenue>>();
            var mockProviderVenueDbSet = new List<ProviderVenue>
                {
                    new ProviderVenue
                    {
                        Postcode = "CV1 2WT",
                        Town = null
                    }
                }
                .AsQueryable()
                .BuildMockDbSet();

            _providerVenueRepository.GetMany(Arg.Any<Expression<Func<ProviderVenue, bool>>>())
                .Returns(mockProviderVenueDbSet);

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            googleMapsApiClient.GetAddressDetailsAsync("CV1 2WT").Returns("Coventry");

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.GetGeoLocationDataAsync(Arg.Any<string>(), Arg.Any<bool>())
                .Returns(new PostcodeLookupResultDto
                    {
                        Postcode = "CV1 2WT",
                        Latitude = "52.400997",
                        Longitude = "-1.508122"
                    });

            var proximityFunctions = new Functions.Proximity(
                _locationApiClient,
                googleMapsApiClient,
                opportunityItemRepository,
                _providerVenueRepository,
                _functionLogRepository);

            proximityFunctions.BackFillProximityDataAsync(
                new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null), 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>() 
                ).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void LocationApiClient_GetGeoLocationDataAsync_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .GetGeoLocationDataAsync(Arg.Is<string>(s => s == "CV1 2WT"),
                    Arg.Is<bool>(b => b));
        }

        [Fact]
        public void ProviderVenueRepository_UpdateMany_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerVenueRepository
                .Received(1)
                .UpdateManyAsync(Arg.Is<IList<ProviderVenue>>(pv => 
                    pv.Count == 1 && 
                    pv.First().Postcode == "CV1 2WT" &&
                    pv.First().Latitude == 52.400997M &&
                    pv.First().Longitude == -1.508122M));
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}