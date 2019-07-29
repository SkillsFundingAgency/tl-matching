using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
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
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
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

            var sut = new ReferralEmailService(config, dateTimeProvider, emailService, emailHistoryService, repo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id, itemIds);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, itemIds, backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Placements_List(
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
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
            var sut = new ReferralEmailService(config, dateTimeProvider, emailService, emailHistoryService, repo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id, itemIds);
            var expectedResult = await GetExpectedResult(repo, opportunity.Id, itemIds);

            var expectedToken = expectedResult.FirstOrDefault(tokens => tokens["opportunity_id"] == opportunity.Id.ToString());

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, itemIds, backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("employer_business_name")), Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == expectedToken["employer_business_name"]), Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Employer_Details(
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration config,
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
            var sut = new ReferralEmailService(config, dateTimeProvider, emailService, emailHistoryService, repo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id, itemIds);
            
            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, itemIds, backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == opportunity.Employer.CompanyName),
                Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_name") && tokens["employer_contact_name"] == opportunity.EmployerContact),
                Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_number") && tokens["employer_contact_number"] == opportunity.EmployerContactPhone),
                Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_email") && tokens["employer_contact_email"] == opportunity.EmployerContactEmail),
                Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Background_Process_History_Status_Is_Completed(
                        MatchingDbContext dbContext,
                        IDateTimeProvider dateTimeProvider,
                        MatchingConfiguration config,
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

            var sut = new ReferralEmailService(config, dateTimeProvider, emailService, emailHistoryService, repo, backgroundRepo);

            var itemIds = itemRepo.GetMany(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id, itemIds);

            //Act
            await sut.SendProviderReferralEmailAsync(opportunity.Id, itemIds, backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            var processStatus = dbContext.BackgroundProcessHistory
                .FirstOrDefault(history => history.Id == backgroundProcessHistory.Id)
                ?.Status;

            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Pending.ToString());
            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Processing.ToString());
            processStatus.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());

        }

        private static async Task<List<IDictionary<string, string>>> GetExpectedResult(IOpportunityRepository repo, int opportunityId, IEnumerable<int> itemIds)
        {
            var providers = await repo.GetProviderOpportunities(opportunityId, itemIds);

            return (from referral in providers
                    let placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements)
                    select new Dictionary<string, string>
                    {
                        {"opportunity_id", referral.OpportunityId.ToString()},
                        {"primary_contact_name", referral.ProviderPrimaryContact},
                        {"provider_name", referral.ProviderName},
                        {"route", referral.RouteName.ToLowerInvariant()},
                        {"venue_postcode", referral.ProviderVenuePostcode},
                        {"search_radius", referral.SearchRadius.ToString()},
                        {"job_role", referral.JobRole},
                        {"employer_business_name", referral.CompanyName},
                        {"employer_contact_name", referral.EmployerContact},
                        {"employer_contact_number", referral.EmployerContactPhone},
                        {"employer_contact_email", referral.EmployerContactEmail},
                        {"employer_postcode", $"{referral.Town} {referral.Postcode}"},
                        {"number_of_placements", placements}
                    }).Cast<IDictionary<string, string>>()
                .ToList();
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }
    }
}
