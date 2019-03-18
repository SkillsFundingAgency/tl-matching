﻿using System;
using System.Net.Http;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Proximity
{
    public class When_GetProximityData_Function_Queue_Trigger_Fires_For_Invalid_PostCode
    {
        private readonly ILogger _logger;

        public When_GetProximityData_Function_Queue_Trigger_Fires_For_Invalid_PostCode()
        {
            var mapper = Substitute.For<IMapper>();
            var locationService = Substitute.For<ILocationService>();
            locationService.GetGeoLocationData("CV1234").Throws(new HttpRequestException("Invalid PostCode"));

            _logger = Substitute.For<ILogger>();
            var proximityfunctions = new Functions.Proximity();
            proximityfunctions.GetProximityData(new GetProximityData { PostCode = "CV1234" }, new ExecutionContext(), _logger, mapper, locationService).GetAwaiter().GetResult();
        }

        [Fact]
        public void Log_Error_Is_Called_Exactly_Once()
        {
            _logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Error Getting Geo Location Data for PostCode: CV1234, Please Check the PostCode, Internal Error Message")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
    }
}