using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Notify.Models.Responses;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.InMemoryDb;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.EmailDeliveryStatus;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_Email_Service_Is_Called_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_And_Save_Email_History(
            MatchingConfiguration configuration,
            [Frozen] MatchingDbContext dbContext,
            IAsyncNotificationClient notificationClient,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<EmailService> emailServiceLogger,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] EmailHistory emailHistory,
            [Frozen] EmailTemplate emailTemplate,
            [Frozen] EmailNotificationResponse emailNotificationResponse
        )
        {
            //Arrange
            var (templateLogger, historyLogger, mapper) = SetUp(dbContext, emailTemplateLogger, emailHistoryLogger);

            var sut = new EmailService(configuration, notificationClient, templateLogger, historyLogger, mapper,
                emailServiceLogger);

            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            notificationClient.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>()).Returns(Task.FromResult(emailNotificationResponse));

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);
            await DataBuilder.SetEmailTemplate(dbContext, emailTemplate);

            var oppItems = opportunity.OpportunityItem.Where(item => item.OpportunityId == opportunity.Id).Select(item => item.Id);


            //Act
            await sut.SendEmailAsync(opportunity.Id, oppItems.FirstOrDefault(), emailTemplate.TemplateName, "test@test.com", tokens, "System");

            //Assert
            Guid.TryParse(emailNotificationResponse.id, out var notificationId);
            var data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            data.Should().NotBeNull();
            data.EmailTemplateId.Should().Be(emailHistory.EmailTemplateId);
            data.Status.Should().BeNullOrEmpty();
            data.NotificationId.Should().Be(emailNotificationResponse.id);
            data.CreatedBy.Should().Be("System");
            data.SentTo.Should().Be("test@test.com");

        }

        [Theory, AutoDomainData]
        public async Task Then_Do_Not_Send_Email_If_Template_Name_Is_Null(
            MatchingConfiguration configuration,
            [Frozen] MatchingDbContext dbContext,
            IAsyncNotificationClient notificationClient,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<EmailService> emailServiceLogger,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            EmailTemplate emailTemplate,
            EmailNotificationResponse emailNotificationResponse
        )
        {
            //Arrange
            var (templateLogger, historyLogger, mapper) = SetUp(dbContext, emailTemplateLogger, emailHistoryLogger);

            var sut = new EmailService(configuration, notificationClient, templateLogger, historyLogger, mapper,
                emailServiceLogger);

            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            notificationClient.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>()).Returns(Task.FromResult(emailNotificationResponse));

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);
            await DataBuilder.SetEmailTemplate(dbContext, emailTemplate);

            var oppItems = opportunity.OpportunityItem.Where(item => item.OpportunityId == opportunity.Id).Select(item => item.Id);


            //Act
            await sut.SendEmailAsync(opportunity.Id, oppItems.FirstOrDefault(), "", "test@test.com", tokens, "System");

            //Assert
            Guid.TryParse(emailNotificationResponse.id, out var notificationId);
            var emailHistory = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            emailHistory.Should().BeNull();

        }

        [Theory, AutoDomainData]
        public async Task Then_Update_Email_History_With_Delivery_Status(
            MatchingConfiguration configuration,
            [Frozen] MatchingDbContext dbContext,
            IAsyncNotificationClient notificationClient,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<EmailService> emailServiceLogger,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            EmailDeliveryStatusPayLoad payLoad,
            EmailHistory emailHistory
        )
        {
            //Arrange
            var (templateLogger, historyLogger, mapper) = SetUp(dbContext, emailTemplateLogger, emailHistoryLogger);

            var sut = new EmailService(configuration, notificationClient, templateLogger, historyLogger, mapper,
                emailServiceLogger);

            emailHistory.NotificationId = payLoad.Id;
            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);
            await DataBuilder.SetEmailHistory(dbContext, emailHistory);

            //Act
            await sut.UpdateEmailStatus(payLoad);

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == payLoad.Id);

            data.Should().NotBeNull();
            data.Status.Should().Be(payLoad.Status);
            data.NotificationId.Should().Be(payLoad.Id);
            data.ModifiedBy.Should().Be("System");

        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("failure-status")]
        [InlineAutoDomainData("temporary-failure-status")]
        [InlineAutoDomainData("temporary-failure-status")]
        public async Task Then_Send_Email_And_Save_Email_History_And_Update_Email_History_With_Status(
            string status,
            MatchingConfiguration configuration,
            [Frozen] MatchingDbContext dbContext,
            IAsyncNotificationClient notificationClient,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<EmailService> emailServiceLogger,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] EmailHistory emailHistory,
            [Frozen] EmailTemplate emailTemplate,
            [Frozen] EmailNotificationResponse emailNotificationResponse,
            EmailDeliveryStatusPayLoad payLoad
        )
        {
            //Arrange
            Guid.TryParse(emailNotificationResponse.id, out var notificationId);
            payLoad.Status = status;
            payLoad.Id = notificationId;

            var (templateLogger, historyLogger, mapper) = SetUp(dbContext, emailTemplateLogger, emailHistoryLogger);

            var sut = new EmailService(configuration, notificationClient, templateLogger, historyLogger, mapper,
                emailServiceLogger);

            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            notificationClient.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>()).Returns(Task.FromResult(emailNotificationResponse));

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);
            await DataBuilder.SetEmailTemplate(dbContext, emailTemplate);
            var oppItems = opportunity.OpportunityItem.Where(item => item.OpportunityId == opportunity.Id).Select(item => item.Id);

            //Act
            await sut.SendEmailAsync(opportunity.Id, oppItems.FirstOrDefault(), emailTemplate.TemplateName, "test@test.com", tokens, "System");
            
            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            data.Should().NotBeNull();
            data.EmailTemplateId.Should().Be(emailHistory.EmailTemplateId);
            data.NotificationId.Should().Be(emailNotificationResponse.id);
            data.CreatedBy.Should().Be("System");
            data.Status.Should().BeNullOrEmpty();
            data.ModifiedBy.Should().BeNullOrEmpty();
            data.ModifiedOn.Should().BeNull();

            //Act - Update Email With Status
            await sut.UpdateEmailStatus(payLoad);

            data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            data.Should().NotBeNull();
            data.EmailTemplateId.Should().Be(emailHistory.EmailTemplateId);
            data.Status.Should().NotBeNullOrEmpty();
            data.Status.Should().Be(payLoad.Status);
            data.NotificationId.Should().Be(emailNotificationResponse.id);
            data.CreatedBy.Should().Be("System");
            data.ModifiedBy.Should().Be("System");

        }

        [Theory]
        [InlineAutoDomainData("delivered")]
        [InlineAutoDomainData("failure-status")]
        [InlineAutoDomainData("temporary-failure-status")]
        [InlineAutoDomainData("temporary-failure-status")]
        public async Task Then_Send_Email_And_Save_Email_History_And_Update_Email_History_If_Status_Is_Null(
            string status,
            MatchingConfiguration configuration,
            [Frozen] MatchingDbContext dbContext,
            IAsyncNotificationClient notificationClient,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger,
            ILogger<EmailService> emailServiceLogger,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] EmailHistory emailHistory,
            [Frozen] EmailTemplate emailTemplate,
            [Frozen] EmailNotificationResponse emailNotificationResponse,
            EmailDeliveryStatusPayLoad payLoad
        )
        {
            //Arrange
            Guid.TryParse(emailNotificationResponse.id, out var notificationId);
            payLoad.Status = status;
            payLoad.Id = notificationId;

            var (templateLogger, historyLogger, mapper) = SetUp(dbContext, emailTemplateLogger, emailHistoryLogger);

            var sut = new EmailService(configuration, notificationClient, templateLogger, historyLogger, mapper,
                emailServiceLogger);

            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            notificationClient.SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>()).Returns(Task.FromResult(emailNotificationResponse));

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);
            await DataBuilder.SetEmailTemplate(dbContext, emailTemplate);
            var oppItems = opportunity.OpportunityItem.Where(item => item.OpportunityId == opportunity.Id).Select(item => item.Id);

            //Act
            await sut.SendEmailAsync(opportunity.Id, oppItems.FirstOrDefault(), emailTemplate.TemplateName, "test@test.com", tokens, "System");

            //Assert
            var data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            data.Should().NotBeNull();
            data.EmailTemplateId.Should().Be(emailHistory.EmailTemplateId);
            data.NotificationId.Should().Be(emailNotificationResponse.id);
            data.CreatedBy.Should().Be("System");
            data.Status.Should().BeNullOrEmpty();
            data.ModifiedBy.Should().BeNullOrEmpty();
            data.ModifiedOn.Should().BeNull();

            //Act - Update Email With Status
            await sut.UpdateEmailStatus(payLoad);

            data = dbContext.EmailHistory.FirstOrDefault(x => x.NotificationId == notificationId);

            data.Should().NotBeNull();
            data.EmailTemplateId.Should().Be(emailHistory.EmailTemplateId);
            data.Status.Should().NotBeNullOrEmpty();
            data.Status.Should().Be(payLoad.Status);
            data.NotificationId.Should().Be(emailNotificationResponse.id);
            data.CreatedBy.Should().Be("System");
            data.ModifiedBy.Should().Be("System");

        }

        private (GenericRepository<EmailTemplate>, GenericRepository<EmailHistory>, Mapper) SetUp(
            MatchingDbContext dbContext,
            ILogger<GenericRepository<EmailTemplate>> emailTemplateLogger,
            ILogger<GenericRepository<EmailHistory>> emailHistoryLogger
        )
        {
            var emailTemplateRepository = new GenericRepository<EmailTemplate>(emailTemplateLogger, dbContext);
            var emailHistoryRepository = new GenericRepository<EmailHistory>(emailHistoryLogger, dbContext);
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            return (emailTemplateRepository, emailHistoryRepository, mapper);
        }
    }
}
