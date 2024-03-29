﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.InMemoryDb;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService
{
    public class When_Email_Delivery_Status_Service_Is_Called_Using_In_Memory_Db
    {
        [Theory, AutoDomainData]
        public async Task Then_Update_Email_History_With_Status(
            MatchingDbContext dbContext,
            MatchingConfiguration configuration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<EmailService> emailServiceLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            IAsyncNotificationClient notificationClient
        )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();

            payload.Id = emailHistory.NotificationId.GetValueOrDefault();

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger,
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            var serializedPayLoad = JsonSerializer.Serialize(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.Id);

            data.Should().NotBeNull();
            data?.NotificationId.Should().Be(payload.Id);
            data?.Status.Should().Be("delivered");
            data?.ModifiedBy.Should().Be("System");

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        [InlineAutoDomainData("not-a-delivered-status-but-invalid-status")]
        public async Task Then_Update_Email_History_With_Status_And_Push_To_Email_Delivery_Status_Queue(
            string status,
            MatchingDbContext dbContext,
            MatchingConfiguration configuration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            ILogger<EmailService> emailServiceLogger,
            IAsyncNotificationClient notificationClient
        )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();

            payload.Status = status;
            payload.Id = emailHistory.NotificationId.GetValueOrDefault();

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger,
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            var serializedPayLoad = JsonSerializer.Serialize(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.Id);

            data.Should().NotBeNull();
            data?.NotificationId.Should().Be(payload.Id);
            data?.Status.Should().Be(payload.Status);
            data?.ModifiedBy.Should().Be("System");

            await messageQueueService.Received(1).PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory]
        [InlineAutoDomainData("")]
        [InlineAutoDomainData(null)]
        public async Task Then_Update_Email_History_With_Status_And_Push_To_Email_Delivery_Status_Queue_If_Status_Is_Null(
            string status,
            MatchingDbContext dbContext,
            MatchingConfiguration configuration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<EmailService> emailServiceLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            IAsyncNotificationClient notificationClient
        )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            await dbContext.SaveChangesAsync();
            dbContext.DetachAllEntities();

            payload.Status = status;
            payload.Id = emailHistory.NotificationId.GetValueOrDefault();

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger,
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            var serializedPayLoad = JsonSerializer.Serialize(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.Id);

            data.Should().NotBeNull();
            data?.NotificationId.Should().Be(payload.Id);
            data?.Status.Should().Be(payload.Status);
            data?.ModifiedBy.Should().Be("System");

            await messageQueueService.Received(1).PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("permanent-failure")]
        [InlineAutoDomainData("temporary-failure")]
        public async Task Then_Do_Not_Update_Email_History_If_PayLoad_Notification_Id_Is_Null(
            string status,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload,
            MatchingConfiguration configuration,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            ILogger<EmailService> emailServiceLogger,
            IAsyncNotificationClient notificationClient
        )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            await dbContext.SaveChangesAsync();

            payload.Status = status;
            payload.Id = Guid.Empty;

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger, 
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            var serializedPayLoad = JsonSerializer.Serialize(payload);

            //Act
            await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.Id);
            data.Should().BeNull();

            var existingData = dbContext.EmailHistory.Where(history => history.OpportunityId == opportunity.Id).ToList();
            existingData.Select(history => history.ModifiedBy).Should().Equal(new List<string> { null, null });
            existingData.Select(history => history.ModifiedOn).Should().Equal(new List<DateTime?> { null, null });

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory]
        [InlineAutoDomainData(null)]
        [InlineAutoDomainData("")]
        [InlineAutoDomainData("")]
        public async Task Then_Do_Not_Update_Email_History_If_PayLoad_Is_Null_Or_Empty(
            string payload,
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] EmailHistory emailHistory,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            IMessageQueueService messageQueueService, MatchingConfiguration configuration,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<EmailService> emailServiceLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            IAsyncNotificationClient notificationClient
            )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            dbContext.Add(emailHistory);
            await dbContext.SaveChangesAsync();

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger,
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(payload);

            //Assert

            result.Should().Be(-1);

            var existingData = dbContext.EmailHistory.Where(history => history.OpportunityId == opportunity.Id).ToList();
            existingData.Select(history => history.ModifiedBy).Should().Equal(new List<string> { null, null });
            existingData.Select(history => history.ModifiedOn).Should().Equal(new List<DateTime?> { null, null });

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Update_Email_History_If_Does_Not_Exists(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            IMessageQueueService messageQueueService,
            EmailDeliveryStatusPayLoad payload,
            MatchingConfiguration configuration,
            ILogger<OpportunityRepository> opportunityRepoLogger,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<EmailService> emailServiceLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            IAsyncNotificationClient notificationClient
        )
        {
            //Arrange
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var sut = SutSetUp(dbContext, opportunityRepoLogger, emailTemplateLogger, emailHistoryLogger, functionLogLogger,
                emailDeliveryServiceStatusLogger, emailServiceLogger, notificationClient, configuration, messageQueueService);

            var serializedPayLoad = JsonSerializer.Serialize(payload);

            //Act
            var result = await sut.HandleEmailDeliveryStatusAsync(serializedPayLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(em => em.NotificationId == payload.Id);
            data.Should().BeNull();
            result.Should().Be(-1);

            await messageQueueService.DidNotReceive().PushEmailDeliveryStatusMessageAsync(Arg.Any<SendEmailDeliveryStatus>());
        }

        private static Application.Services.EmailDeliveryStatusService SutSetUp(
            MatchingDbContext dbContext, 
            ILogger<OpportunityRepository> opportunityRepoLogger,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger,
            ILogger<Application.Services.EmailDeliveryStatusService> emailDeliveryServiceStatusLogger,
            ILogger<EmailService> emailServiceLogger,
            IAsyncNotificationClient notificationClient, 
            MatchingConfiguration configuration, 
            IMessageQueueService messageQueueService)
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailService).Assembly));
            var mapper = new Mapper(config);

            var emailTemplateRepository = new GenericRepository<EmailTemplate>(emailTemplateLogger, dbContext);
            var opportunityRepository = new OpportunityRepository(opportunityRepoLogger, dbContext);
            var emailHistoryRepository = new GenericRepository<EmailHistory>(emailHistoryLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);
            var emailService = new EmailService(configuration, notificationClient, emailTemplateRepository, emailHistoryRepository, functionLogRepository, mapper, emailServiceLogger);

            return new Application.Services.EmailDeliveryStatusService(configuration,
                emailService, opportunityRepository, messageQueueService, emailDeliveryServiceStatusLogger);
        }
    }
}