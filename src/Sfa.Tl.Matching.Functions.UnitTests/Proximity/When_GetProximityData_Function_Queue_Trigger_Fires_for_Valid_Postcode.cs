using System.Net.Http;
using AutoMapper;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_GetProximityData_Function_Queue_Trigger_Fires_For_Valid_Postcode
    {
        private readonly SaveProximityData _result;

        public When_GetProximityData_Function_Queue_Trigger_Fires_For_Valid_Postcode()
        {
            var mapper = Substitute.For<IMapper>();
            
            var locationService = new LocationService(
                new HttpClient(), 
                new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes"
                });
            
            var proximityfunctions = new Functions.Proximity();
           _result = proximityfunctions.GetProximityData(new GetProximityData { Postcode = "CV12WT" }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, locationService).GetAwaiter().GetResult();
        }

        [Fact]
        public void SaveProximityData_contains_Formated_PostCode()
        {
            _result.Postcode.Should().Be("CV1 2WT");
        }
    }
}