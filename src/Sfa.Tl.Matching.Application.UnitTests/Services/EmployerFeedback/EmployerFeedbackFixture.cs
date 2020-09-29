using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class EmployerFeedbackFixture
    {
        internal readonly MatchingConfiguration Configuration;
        internal readonly ILogger<EmployerFeedbackService> Logger;

        public EmployerFeedbackFixture()
        {
            Configuration = new MatchingConfiguration
            {
                EmployerFeedbackEmailsEnabled = true,
                EmployerFeedbackWorkingDayInMonth = 10,
                SendEmailEnabled = true
            };

            Logger = Substitute.For<ILogger<EmployerFeedbackService>>();
        }

        public bool DoTokensContainExpectedValues(IDictionary<string, string> tokens, IDictionary<string, string> values)
        {
            return tokens != null && 
                   values.All(value => tokens.ContainsKey(value.Key) && tokens[value.Key] == value.Value);
        }
    }
}
