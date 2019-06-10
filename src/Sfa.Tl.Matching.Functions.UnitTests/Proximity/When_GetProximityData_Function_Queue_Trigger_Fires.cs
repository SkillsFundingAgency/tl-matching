using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_GetProximityData_Function_Queue_Trigger_Fires
    {
        private readonly ILocationApiClient _locationApiClient;

        public When_GetProximityData_Function_Queue_Trigger_Fires()
        {
            var mapper = Substitute.For<IMapper>();
            _locationApiClient = Substitute.For<ILocationApiClient>();

            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.GetProximityData(new GetProximityData { Postcode = "CV12WT" }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, _locationApiClient, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .GetGeoLocationData(Arg.Is<string>(s => s == "CV12WT"));
        }
    }
}