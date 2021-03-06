﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.InMemoryDb;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail
{
    public class When_ReferralEmailService_Is_Called_To_Send_Provider_Email_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Providers(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Placements_List(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("employer_business_name")), Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Employer_Details(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Any<IDictionary<string, string>>(),
                Arg.Any<string>());

            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == opportunity.Employer.CompanyName.ToTitleCase()),
            Arg.Any<string>());

            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_name") && tokens["employer_contact_name"] == opportunity.EmployerContact.ToTitleCase()), 
                Arg.Any<string>());

            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                tokens.ContainsKey("employer_contact_number") && tokens["employer_contact_number"] == opportunity.EmployerContactPhone),
                Arg.Any<string>());

            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                tokens.ContainsKey("employer_contact_email") && tokens["employer_contact_email"] == opportunity.EmployerContactEmail),
                Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Background_Process_History_Status_Is_Completed(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
                        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(4).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            var processStatus = dbContext.BackgroundProcessHistory
                .FirstOrDefault(history => history.Id == backgroundProcessHistory.Id)
                ?.Status;

            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Pending.ToString());
            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Processing.ToString());
            processStatus.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());
        }

        [Theory]
        [InlineAutoDomainData("", "")]
        [InlineAutoDomainData("Test", "")]
        [InlineAutoDomainData("", "Test")]
        [InlineAutoDomainData(null, null)]
        [InlineAutoDomainData(null, "")]
        [InlineAutoDomainData("", null)]
        [InlineAutoDomainData("Test", null)]
        [InlineAutoDomainData(null, "Test")]
        //[InlineData("Test", "Test")]
        public async Task AND_Secondary_Contact_Name_or_Email_Is_Invalid_Then_Send_Email_Is_NOT_Called(
            string secondaryContactName,
            string secondaryContactEmail,
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            provider.SecondaryContact = secondaryContactName;
            provider.SecondaryContactEmail = secondaryContactEmail;

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(2).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(),
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("employer_business_name")),
                Arg.Any<string>());
        }
    }
}