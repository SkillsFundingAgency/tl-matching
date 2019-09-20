﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
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
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Feedback_Email
    {
        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Providers(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            ILogger<GenericRepository<BankHoliday>> bankHolidayLogger,
            ILogger<OpportunityRepository> opportunityLogger,
            ILogger<GenericRepository<OpportunityItem>> opportunityItemLogger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue
        )
        {
            //Arrange
            var bankHolidayRepo = new GenericRepository<BankHoliday>(bankHolidayLogger, dbContext);
            var opportunityRepo = new OpportunityRepository(opportunityLogger, dbContext);
            var opportunityItemRepo = new GenericRepository<OpportunityItem>(opportunityItemLogger, dbContext);

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<OpportunityItemWithUsernameForProviderFeedbackSentDto, OpportunityItem>(
                            dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-09-19 23:59:59"));

            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            await ProviderFeedbackInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity);

            var sut = new ProviderFeedbackService(mapper, configuration, logger, dateTimeProvider, emailService,
                emailHistoryService, bankHolidayRepo, opportunityRepo, opportunityItemRepo);

            //Act
            var emailsCount = await sut.SendProviderFeedbackEmailsAsync("test system");

            //Assert
            emailsCount.Should().Be(1);

            await emailService.Received(1)
                .SendEmail(Arg.Is<string>(
                    templateName => templateName == "ProviderFeedback"),
                Arg.Is<string>(toAddress => toAddress == provider.PrimaryContactEmail),
                Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                Arg.Any<IDictionary<string, string>>(),
                Arg.Is<string>(replyToAddress => replyToAddress == ""));

            await emailService.Received(1)
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "ProviderFeedback"),
                    Arg.Is<string>(toAddress => toAddress == provider.SecondaryContactEmail),
                    Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(replyToAddress => replyToAddress == ""));
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Only_To_Primary_Providers(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            ILogger<GenericRepository<BankHoliday>> bankHolidayLogger,
            ILogger<OpportunityRepository> opportunityLogger,
            ILogger<GenericRepository<OpportunityItem>> opportunityItemLogger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue
        )
        {
            //Arrange
            var bankHolidayRepo = new GenericRepository<BankHoliday>(bankHolidayLogger, dbContext);
            var opportunityRepo = new OpportunityRepository(opportunityLogger, dbContext);
            var opportunityItemRepo = new GenericRepository<OpportunityItem>(opportunityItemLogger, dbContext);

            provider.SecondaryContactEmail = string.Empty;

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<OpportunityItemWithUsernameForProviderFeedbackSentDto, OpportunityItem>(
                            dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-09-19 23:59:59"));

            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            var expectedResults = new Dictionary<string, string>
            {
                {"employer_contact_name", "Employer Contact"},
            };

            await ProviderFeedbackInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity);

            var sut = new ProviderFeedbackService(mapper, configuration, logger, dateTimeProvider, emailService,
                emailHistoryService, bankHolidayRepo, opportunityRepo, opportunityItemRepo);

            //Act
            var emailsCount = await sut.SendProviderFeedbackEmailsAsync("test system");

            //Assert
            emailsCount.Should().Be(1);

            await emailService.Received(1)
                .SendEmail(Arg.Is<string>(
                    templateName => templateName == "ProviderFeedback"),
                Arg.Is<string>(toAddress => toAddress == provider.PrimaryContactEmail),
                Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                Arg.Any<IDictionary<string, string>>(),
                Arg.Is<string>(replyToAddress => replyToAddress == ""));

            await emailService.DidNotReceive()
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "ProviderFeedback"),
                    Arg.Is<string>(toAddress => toAddress == provider.SecondaryContactEmail),
                    Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(replyToAddress => replyToAddress == ""));
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Providers_with_Proper_Tokens(
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            ILogger<GenericRepository<BankHoliday>> bankHolidayLogger,
            ILogger<OpportunityRepository> opportunityLogger,
            ILogger<GenericRepository<OpportunityItem>> opportunityItemLogger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue
        )
        {
            //Arrange
            var bankHolidayRepo = new GenericRepository<BankHoliday>(bankHolidayLogger, dbContext);
            var opportunityRepo = new OpportunityRepository(opportunityLogger, dbContext);
            var opportunityItemRepo = new GenericRepository<OpportunityItem>(opportunityItemLogger, dbContext);

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("UtcNowResolver")
                        ? new UtcNowResolver<OpportunityItemWithUsernameForProviderFeedbackSentDto, OpportunityItem>(
                            dateTimeProvider)
                        : null);
            });
            var mapper = new Mapper(config);

            dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-09-19 23:59:59"));

            dateTimeProvider
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                .Returns(false);

            await ProviderFeedbackInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity);

            var sut = new ProviderFeedbackService(mapper, configuration, logger, dateTimeProvider, emailService,
                emailHistoryService, bankHolidayRepo, opportunityRepo, opportunityItemRepo);

            //Act
            var emailsCount = await sut.SendProviderFeedbackEmailsAsync("test system");

            //Assert
            emailsCount.Should().Be(1);

            await emailService.Received(1)
                .SendEmail(Arg.Is<string>(
                    templateName => templateName == "ProviderFeedback"),
                Arg.Is<string>(toAddress => toAddress == provider.PrimaryContactEmail),
                Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                Arg.Any<IDictionary<string, string>>(),
                Arg.Is<string>(replyToAddress => replyToAddress == ""));

            await emailService.Received(2).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("contact_name") && tokens["contact_name"] == $"{provider.PrimaryContact} / {provider.SecondaryContact}"), Arg.Any<string>());
        }

        private bool GetExpectedTokens(IDictionary<string, string> tokens, IDictionary<string, string> expectedResults)
        {
            if (tokens == null)
            {
                return false;
            }

            return expectedResults.All(expectedResult =>
                tokens.ContainsKey(expectedResult.Key) && tokens[expectedResult.Key] == expectedResult.Value);
        }
    }
}
