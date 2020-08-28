using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderQuarterlyUpdateEmailService
{
    public class ProviderQuarterlyUpdateEmailFixture
    {
        internal readonly MatchingConfiguration Configuration;
        internal readonly IDateTimeProvider DateTimeProvider;
        internal readonly ILogger<Application.Services.ProviderQuarterlyUpdateEmailService> Logger;

        public ProviderQuarterlyUpdateEmailFixture()
        {
            Configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            DateTimeProvider = Substitute.For<IDateTimeProvider>();
            Logger = Substitute.For<ILogger<Application.Services.ProviderQuarterlyUpdateEmailService>>();
        }

        public bool DoTokensContainExpectedValues(IDictionary<string, string> tokens, IDictionary<string, string> values)
        {
            return tokens != null && 
                   values.All(value => tokens.ContainsKey(value.Key) && tokens[value.Key] == value.Value);
        }
    }
}
