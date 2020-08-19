using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ReferralEmails
{
    public class When_SendProviderReferralEmails_Function_Queue_Trigger_Fires
    {
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IReferralEmailService _referralEmailService;

        public When_SendProviderReferralEmails_Function_Queue_Trigger_Fires()
        {
            _referralEmailService = Substitute.For<IReferralEmailService>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var data = new SendProviderReferralEmail
            {
                OpportunityId = 1,
                ItemIds = new List<int> { 101 },
                BackgroundProcessHistoryId = 10
            };

            var providerQuarterlyUpdateEmailFunctions = new Functions.ReferralEmails(_referralEmailService,
                _functionLogRepository);

            providerQuarterlyUpdateEmailFunctions.SendProviderReferralEmailsAsync(
                data,
                new ExecutionContext(),
                new NullLogger<Functions.ReferralEmails>()
            ).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendProviderReferralEmailAsync_Is_Called_Exactly_Once()
        {
            _referralEmailService
                .Received(1)
                .SendProviderReferralEmailAsync(
                    Arg.Any<int>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<int>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendProviderReferralEmailAsync_Is_Called_Exactly_Once_With_Expected_Parameters()
        {
            _referralEmailService
                .Received(1)
                .SendProviderReferralEmailAsync(
                    Arg.Any<int>(),
                    Arg.Any<IEnumerable<int>>(),
                    Arg.Any<int>(),
                    Arg.Is<string>(u => u == "System"));
        }

        [Fact]
        public void SendProviderReferralEmailAsync_Is_Called_Exactly_Once_With_Expected_Parameters_2()
        {
            _referralEmailService
                .Received(1)
                .SendProviderReferralEmailAsync(
                    Arg.Is<int>(id => id == 1),
                    // ReSharper disable once PossibleMultipleEnumeration
                    Arg.Is<IEnumerable<int>>(p => p.Count() == 1 && p.First() == 101),
                    Arg.Is<int>(id => id == 10),
                    Arg.Is<string>(u => u == "System"));
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}