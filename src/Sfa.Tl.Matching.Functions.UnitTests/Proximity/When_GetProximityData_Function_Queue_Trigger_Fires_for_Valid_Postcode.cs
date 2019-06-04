using AutoMapper;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_GetProximityData_Function_Queue_Trigger_Fires_For_Valid_Postcode
    {
        private readonly SaveProximityData _result;

        public When_GetProximityData_Function_Queue_Trigger_Fires_For_Valid_Postcode()
        {
            var httpClient = new PostcodesIoHttpClient().Get();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            var locationService = new LocationService(
                httpClient, 
                new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes"
                });
            
            var proximityfunctions = new Functions.Proximity();
           _result = proximityfunctions.GetProximityData(new GetProximityData
           {
               Postcode = "CV12WT"
           }, new ExecutionContext(), new NullLogger<Functions.Proximity>(), mapper, locationService, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void SaveProximityData_contains_Formated_PostCode()
        {
            _result.Postcode.Should().Be("CV1 2WT");
        }
    }
}