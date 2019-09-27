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
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IGoogleMapApiClient _googleMapsApiClient;
        private readonly ILocationApiClient _locationApiClient;
        public When_Proximity_BackFillEmployerPostTown_Function_Fires()
        {
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(new List<OpportunityItem>{ new OpportunityItem
                {
                    Postcode = "CV1 2WT",
                    Town = null
                }}.AsQueryable());

            _googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapsApiClient.GetAddressDetails("CV1 2WT").Returns("Coventry");

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.IsValidPostcodeAsync(Arg.Any<string>(), Arg.Any<bool>()).Returns((true, "CV1 2WT"));

            var proximityfunctions = new Functions.Proximity();

            proximityfunctions.BackFillEmployerPostTown(new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null), new ExecutionContext(), new NullLogger<Functions.Proximity>(), _locationApiClient, _googleMapsApiClient, _opportunityItemRepository, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
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
                .GetAddressDetails(Arg.Is<string>(s => s == "CV1 2WT"));
        }

        [Fact]
        public void OpportunityItemRepository_UpdateMany_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .UpdateMany(Arg.Is<IList<OpportunityItem>>(pv => pv.Count == 1 && pv.First().Town == "Coventry"));
        }
    }
}