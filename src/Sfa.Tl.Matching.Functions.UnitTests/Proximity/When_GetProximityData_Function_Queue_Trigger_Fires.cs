using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_GetProximityData_Function_Queue_Trigger_Fires
    {
        private readonly ILocationService _locationService;

        public When_GetProximityData_Function_Queue_Trigger_Fires()
        {
            var mapper = Substitute.For<IMapper>();
            _locationService = Substitute.For<ILocationService>();

            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.GetProximityData(new GetProximityData { PostCode = "CV12WT" }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, _locationService).GetAwaiter().GetResult();
        }

        [Fact]
        public void GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _locationService
                .Received(1)
                .GetGeoLocationData(Arg.Is<string>(s => s == "CV12WT"));
        }
    }
}