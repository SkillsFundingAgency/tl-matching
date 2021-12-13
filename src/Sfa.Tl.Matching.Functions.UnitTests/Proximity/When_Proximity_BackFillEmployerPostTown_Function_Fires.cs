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
    public class When_Proximity_BackFillEmployerPostTown_Function_Fires
    {
        private readonly ILocationApiClient _locationApiClient;
        private readonly IGoogleMapApiClient _googleMapsApiClient;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_Proximity_BackFillEmployerPostTown_Function_Fires()
        {
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(new List<OpportunityItem>
                { 
                    new()
                    {
                        Postcode = "CV1 2WT",
                        Town = null
                    }
                }.AsQueryable());

            var providerVenueRepository = Substitute.For<IRepository<ProviderVenue>>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            _googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapsApiClient.GetAddressDetailsAsync("CV1 2WT").Returns("Coventry");

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.IsValidPostcodeAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns((true, "CV1 2WT"));

            var proximityFunctions = new Functions.Proximity(
                _locationApiClient,
                _googleMapsApiClient,
                _opportunityItemRepository,
                providerVenueRepository,
                _functionLogRepository);

            proximityFunctions.BackFillEmployerPostTownAsync(
                new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null), 
                new ExecutionContext(), 
                new NullLogger<Functions.Proximity>()
                ).GetAwaiter().GetResult();
        }

        [Fact]
        public void LocationApiClient_IsValidPostcode_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .IsValidPostcodeAsync(Arg.Is<string>(s => s == "CV1 2WT"),
                    Arg.Is<bool>(b => b));
        }

        [Fact]
        public void GoogleMapsApiClient_GetAddressDetails_Is_Called_Exactly_Once()
        {
            _googleMapsApiClient
                .Received(1)
                .GetAddressDetailsAsync(Arg.Is<string>(s => s == "CV1 2WT"));
        }

        [Fact]
        public void OpportunityItemRepository_UpdateMany_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .UpdateManyAsync(Arg.Is<IList<OpportunityItem>>(pv => pv.Count == 1 && pv.First().Town == "Coventry"));
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