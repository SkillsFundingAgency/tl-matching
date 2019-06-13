using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_Proximity_BackFillPostTown_Function_Fires
    {
        private readonly IRepository<ProviderVenue> _providerVenueRepository;
        private readonly IGoogleMapApiClient _googleMapsApiClient;

        public When_Proximity_BackFillPostTown_Function_Fires()
        {
            _providerVenueRepository = Substitute.For<IRepository<ProviderVenue>>();
            _providerVenueRepository.GetMany(Arg.Any<Expression<Func<ProviderVenue, bool>>>())
                .Returns(new List<ProviderVenue>{ new ProviderVenue
                {
                    Postcode = "CV1 2WT",
                    Town = null,
                }}.AsQueryable());

            _googleMapsApiClient = Substitute.For<IGoogleMapApiClient>();
            _googleMapsApiClient.GetAddressDetails("CV1 2WT").Returns("Coventry");

            var proximityfunctions = new Functions.Proximity();

            proximityfunctions.BackFillPostTown(new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null), new ExecutionContext(), new NullLogger<Functions.Proximity>(), _googleMapsApiClient, _providerVenueRepository, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _googleMapsApiClient
                .Received(1)
                .GetAddressDetails(Arg.Is<string>(s => s == "CV1 2WT"));
        }

        [Fact]
        public void ProviderVenueRepository_UpdateMany_Is_Called_Exactly_Once()
        {
            _providerVenueRepository
                .Received(1)
                .UpdateMany(Arg.Is<IList<ProviderVenue>>(pv => pv.Count == 1 && pv.First().Town == "Coventry"));
        }
    }
}