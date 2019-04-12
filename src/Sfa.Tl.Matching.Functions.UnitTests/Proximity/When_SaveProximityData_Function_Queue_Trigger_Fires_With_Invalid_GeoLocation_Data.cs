using System;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_SaveProximityData_Function_Queue_Trigger_Fires_With_Invalid_GeoLocation_Data
    {
        private readonly IRepository<Domain.Models.ProviderVenue> _providerVenueRepository;
        private readonly ILogger _logger;

        public When_SaveProximityData_Function_Queue_Trigger_Fires_With_Invalid_GeoLocation_Data()
        {
            var mapper = Substitute.For<IMapper>();
            _providerVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            _logger = Substitute.For<ILogger>();

            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.SaveProximityData(new SaveProximityData { Postcode = "CV12WT", ProviderVenueId = 123 }, new ExecutionContext(), _logger, mapper, _providerVenueRepository, Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Update_ProviderVenue_Is_Not_Called()
        {
            _providerVenueRepository
                .DidNotReceive()
                .Update(Arg.Any<Domain.Models.ProviderVenue>());
        }

        [Fact]
        public void Log_Error_Is_Called_Exactly_Once()
        {
            _logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}