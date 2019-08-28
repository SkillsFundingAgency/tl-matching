using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralEmailService_Is_Called_To_Send_Provider_Email_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Providers(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);

            var sut = new ReferralEmailService(mapper, config, dateTimeProvider, emailService, emailHistoryService, repo, itemRepo, backgroundRepo);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");


            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral).Select(oi => oi.Id);

            //Assert
            var data = (from op in dbContext.Opportunity
                        join oi in dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                        join emp in dbContext.Employer on op.EmployerId equals emp.Id
                        join re in dbContext.Referral on oi.Id equals re.OpportunityItemId
                        join pv in dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                        join p in dbContext.Provider on pv.ProviderId equals p.Id
                        join r in dbContext.Route on oi.RouteId equals r.Id
                        orderby re.DistanceFromEmployer
                        where op.Id == opportunity.Id
                              && itemIds.Contains(oi.Id)
                              && oi.IsSelectedForReferral
                              && oi.IsSaved
                              && p.IsCdfProvider
                              && p.IsEnabledForReferral
                              && pv.IsEnabledForReferral
                              && !pv.IsRemoved
                        select oi.Id
                ).ToList();

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Placements_List(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);

            var sut = new ReferralEmailService(mapper, config, dateTimeProvider, emailService, emailHistoryService, repo, itemRepo, backgroundRepo);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");


            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral).Select(oi => oi.Id);

            var data = (from op in dbContext.Opportunity
                        join oi in dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                        join emp in dbContext.Employer on op.EmployerId equals emp.Id
                        join re in dbContext.Referral on oi.Id equals re.OpportunityItemId
                        join pv in dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                        join p in dbContext.Provider on pv.ProviderId equals p.Id
                        join r in dbContext.Route on oi.RouteId equals r.Id
                        orderby re.DistanceFromEmployer
                        where op.Id == opportunity.Id
                              && itemIds.Contains(oi.Id)
                              && oi.IsSelectedForReferral
                              && oi.IsSaved
                              && p.IsCdfProvider
                              && p.IsEnabledForReferral
                              && pv.IsEnabledForReferral
                              && !pv.IsRemoved
                        select oi.Id
                ).ToList();

            //Assert
            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("employer_business_name")), Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Employer_Details(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            
            var sut = new ReferralEmailService(mapper, config, dateTimeProvider, emailService, emailHistoryService, repo, itemRepo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral).Select(oi => oi.Id);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            var data = (from op in dbContext.Opportunity
                        join oi in dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                        join emp in dbContext.Employer on op.EmployerId equals emp.Id
                        join re in dbContext.Referral on oi.Id equals re.OpportunityItemId
                        join pv in dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                        join p in dbContext.Provider on pv.ProviderId equals p.Id
                        join r in dbContext.Route on oi.RouteId equals r.Id
                        orderby re.DistanceFromEmployer
                        where op.Id == opportunity.Id
                              && itemIds.Contains(oi.Id)
                              && oi.IsSelectedForReferral
                              && oi.IsSaved
                              && p.IsCdfProvider
                              && p.IsEnabledForReferral
                              && pv.IsEnabledForReferral
                              && !pv.IsRemoved
                        select oi.Id
                ).ToList();

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == opportunity.Employer.CompanyName),
                Arg.Any<string>());

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_name") && tokens["employer_contact_name"] == opportunity.EmployerContact),
                Arg.Any<string>());

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_number") && tokens["employer_contact_number"] == opportunity.EmployerContactPhone),
                Arg.Any<string>());

            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_email") && tokens["employer_contact_email"] == opportunity.EmployerContactEmail),
                Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Background_Process_History_Status_Is_Completed(
            [Frozen] MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
            [Frozen] Mapper mapper,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger
                        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);

            var sut = new ReferralEmailService(mapper, config, dateTimeProvider, emailService, emailHistoryService, repo, itemRepo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var referrals = (from op in dbContext.Opportunity
                    join oi in dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                    join emp in dbContext.Employer on op.EmployerId equals emp.Id
                    join re in dbContext.Referral on oi.Id equals re.OpportunityItemId
                    join pv in dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                    join p in dbContext.Provider on pv.ProviderId equals p.Id
                    join r in dbContext.Route on oi.RouteId equals r.Id
                    orderby re.DistanceFromEmployer
                    where op.Id == opportunity.Id
                          && itemIds.Contains(oi.Id)
                          && oi.IsSelectedForReferral
                          && oi.IsSaved
                          && p.IsCdfProvider
                          && p.IsEnabledForReferral
                          && pv.IsEnabledForReferral
                          && !pv.IsRemoved
                    select oi.Id
                ).ToList();

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(4).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            var processStatus = dbContext.BackgroundProcessHistory
                .FirstOrDefault(history => history.Id == backgroundProcessHistory.Id)
                ?.Status;

            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Pending.ToString());
            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Processing.ToString());
            processStatus.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());
        }
    }
}