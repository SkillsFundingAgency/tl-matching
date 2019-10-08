using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_Not_To_Send_Email_Again
    {
        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Send_Email_Again(
            MatchingConfiguration configuration,
            HttpContextAccessor httpContextAccessor,
            ILogger<ProviderFeedbackService> logger,
            ILogger<GenericRepository<BankHoliday>> bankHolidayLogger,
            ILogger<OpportunityRepository> opportunityLogger,
            ILogger<GenericRepository<Domain.Models.Provider>> opportunityItemLogger,
            ILogger<GenericRepository<BackgroundProcessHistory>> backgroundHistoryLogger,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue
        )
        {
            //Arrange
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var bankHolidayRepo = new GenericRepository<BankHoliday>(bankHolidayLogger, dbContext);
            var opportunityRepo = new OpportunityRepository(opportunityLogger, dbContext);
            var opportunityItemRepo = new GenericRepository<Domain.Models.Provider>(opportunityItemLogger, dbContext);
            var backgroundProcessHistoryRepository = new GenericRepository<BackgroundProcessHistory>(
                backgroundHistoryLogger, dbContext);

            var mapper =
                MapperConfig<ProviderMapper, UsernameForFeedbackSentDto, Domain.Models.Provider>.CreateMapper(
                    httpContextAccessor, dateTimeProvider);

            provider.ProviderFeedbackSentOn = DateTime.UtcNow;

            dateTimeProvider
                .GetReferralDateAsync(Arg.Any<IList<DateTime>>(), Arg.Any<string>())
                .Returns(DateTime.Parse("2019-09-19 23:59:59"));

            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            await ProviderFeedbackInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity);

            var sut = new ProviderFeedbackService(mapper, configuration, logger, dateTimeProvider, emailService,
                emailHistoryService, bankHolidayRepo, opportunityRepo, opportunityItemRepo, backgroundProcessHistoryRepository);

            //Act
            var emailsCount = await sut.SendFeedbackEmailsAsync("test system");

            //Assert
            emailsCount.Should().Be(0);

            await emailService.DidNotReceive()
                .SendEmailAsync(Arg.Is<string>(
                        templateName => templateName == "ProviderFeedback"),
                Arg.Is<string>(toAddress => toAddress == provider.PrimaryContactEmail),
                Arg.Any<IDictionary<string, string>>());

            await emailService.DidNotReceive()
                .SendEmailAsync(Arg.Is<string>(
                        templateName => templateName == "ProviderFeedback"),
                    Arg.Is<string>(toAddress => toAddress == provider.SecondaryContactEmail),
                    Arg.Any<IDictionary<string, string>>());
        }

        
    }
}
