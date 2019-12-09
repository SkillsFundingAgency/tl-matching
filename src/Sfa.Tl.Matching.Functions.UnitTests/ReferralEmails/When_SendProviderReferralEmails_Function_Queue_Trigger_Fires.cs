using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.ReferralEmails
{
    public class When_SendProviderReferralEmails_Function_Queue_Trigger_Fires
    {
        [Theory, AutoDomainData]
        public async void Should_Send_Referral_Emails_To_Provider(
                        IReferralEmailService referralEmailService, 
                        IRepository<FunctionLog> functionLogRepo,
                        SendProviderReferralEmail data,
                        ExecutionContext context,
                        ILogger logger
        )
        {
            //Arrange
            var providerReferralEmailFunctions = new Functions.ReferralEmails();

            //Act
            await providerReferralEmailFunctions.SendProviderReferralEmailsAsync(data, context, logger, referralEmailService,
                functionLogRepo);

            //Assert
            await referralEmailService.Received(1).SendProviderReferralEmailAsync(Arg.Any<int>(), Arg.Any<IList<int>>(), Arg.Any<int>(), Arg.Any<string>());

        }
    }
}