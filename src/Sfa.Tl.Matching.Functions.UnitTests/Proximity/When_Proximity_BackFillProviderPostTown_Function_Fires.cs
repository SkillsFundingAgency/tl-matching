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
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_Proximity_BackFillProviderPostTown_Function_Fires
    {
        private readonly IGoogleMapApiClient _googleMapsApiClient;
        private readonly IRepository<ProviderVenue> _providerVenueRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_Proximity_BackFillProviderPostTown_Function_Fires()
        {
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            _providerVenueRepository = Substitute.For<IRepository<ProviderVenue>>();
            _providerVenueRepository.GetManyAsync(Arg.Any<Expression<Func<ProviderVenue, bool>>>())
                .Returns(new List<ProviderVenue>{ new ProviderVenue
                {
                    Postcode = "CV1 2WT",
                    Town = null
                }}.AsQueryable());

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            _googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapsApiClient.GetAddressDetailsAsync("CV1 2WT").Returns("Coventry");

            var locationApiClient = Substitute.For<ILocationApiClient>();
            
            var proximityFunctions = new Functions.Proximity(
                locationApiClient,
                _googleMapsApiClient,
                opportunityItemRepository,
                _providerVenueRepository,
                _functionLogRepository);

            proximityFunctions.BackFillProviderPostTownAsync(
                new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null), 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>() 
                ).GetAwaiter().GetResult();
        }

        [Fact]
        public void GetAddressDetails_Is_Called_Exactly_Once()
        {
            _googleMapsApiClient
                .Received(1)
                .GetAddressDetailsAsync(Arg.Is<string>(s => s == "CV1 2WT"));
        }

        [Fact]
        public void ProviderVenueRepository_UpdateMany_Is_Called_Exactly_Once()
        {
            _providerVenueRepository
                .Received(1)
                .UpdateManyAsync(Arg.Is<IList<ProviderVenue>>(pv => pv.Count == 1 && pv.First().Town == "Coventry"));
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