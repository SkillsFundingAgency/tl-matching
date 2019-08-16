﻿using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class EmployerFeedbackFixture
    {
       internal readonly MatchingConfiguration Configuration;
       internal readonly  IDateTimeProvider DateTimeProvider;
       internal readonly ILogger<EmployerFeedbackService> Logger;

        public EmployerFeedbackFixture()
        {
            Configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                NotificationsSystemId = "TLevelsIndustryPlacement"
            };

            DateTimeProvider = Substitute.For<IDateTimeProvider>();
            Logger = Substitute.For<ILogger<EmployerFeedbackService>>();

        }

        public bool DoTokensContainExpectedValues(IDictionary<string, string> tokens, IDictionary<string, string> expectedResults)
        {
            if (tokens == null)
            {
                return false;
            }

            foreach (var expectedResult in expectedResults)
            {
                if (!(tokens.ContainsKey(expectedResult.Key) &&
                    tokens[expectedResult.Key] == expectedResult.Value))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
