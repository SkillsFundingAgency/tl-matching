using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Provider_Email_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Providers(
                        MatchingDbContext dbContext,
                        [Frozen] Domain.Models.Opportunity opportunity,
                        [Frozen] Domain.Models.Provider provider,
                        [Frozen] Domain.Models.ProviderVenue venue,
                        MatchingConfiguration config,
                        [Frozen] IEmailService emailService,
                        [Frozen] IEmailHistoryService emailHistoryService,
                        ILogger<OpportunityRepository> logger
        )
        {
            //Arrange
            await SetTestData(dbContext, provider, venue, opportunity);

            var repo = new OpportunityRepository(logger, dbContext);
            var sut = new ReferralService(config, emailService, emailHistoryService, repo);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id);

            //Act
            await sut.SendProviderReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Placements_List(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            MatchingConfiguration config,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger
        )
        {
            //Arrange
            await SetTestData(dbContext, provider, venue, opportunity);

            var repo = new OpportunityRepository(logger, dbContext);
            var sut = new ReferralService(config, emailService, emailHistoryService, repo);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id);
            var expectedResult = await GetExpectedResult(repo, opportunity.Id);

            var expectedToken = expectedResult.FirstOrDefault(tokens => tokens["opportunity_id"] == opportunity.Id.ToString());

            //Act
            await sut.SendProviderReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("employer_business_name")), Arg.Any<string>());

            await emailService.Received(referrals.Count).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == expectedToken["employer_business_name"] ), Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Employer_Details(
            MatchingDbContext dbContext,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            MatchingConfiguration config,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<OpportunityRepository> logger
        )
        {
            //Arrange
            await SetTestData(dbContext, provider, venue, opportunity);

            var repo = new OpportunityRepository(logger, dbContext);
            var sut = new ReferralService(config, emailService, emailHistoryService, repo);

            var referrals = await repo.GetProviderOpportunities(opportunity.Id);
            var expectedResult = await GetExpectedResult(repo, opportunity.Id);

            var expectedToken = expectedResult.FirstOrDefault(tokens => tokens["opportunity_id"] == opportunity.Id.ToString());

            //Act
            await sut.SendProviderReferralEmail(opportunity.Id);

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


        private static async Task SetTestData(MatchingDbContext dbContext,
                                    Domain.Models.Provider provider,
                                    Domain.Models.ProviderVenue venue,
                                    Domain.Models.Opportunity opportunity, bool isSaved = true, bool isSelectedForReferral = true)
        {
            await dbContext.AddAsync(provider);
            await dbContext.AddAsync(venue);
            await dbContext.AddAsync(opportunity);
            await dbContext.SaveChangesAsync();

            var items = dbContext.OpportunityItem.Where(oi => oi.OpportunityId == opportunity.Id).AsNoTracking()
                .ToList();

            foreach (var opportunityItem in items)
            {
                opportunityItem.IsSaved = isSaved;
                opportunityItem.IsCompleted = false;
                opportunityItem.IsSelectedForReferral = isSelectedForReferral;
                
                dbContext.Entry(opportunityItem).Property("IsSaved").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsCompleted").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsSelectedForReferral").IsModified = true;
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task<List<IDictionary<string, string>>> GetExpectedResult(OpportunityRepository repo, int opportunityId)
        {
            var providers = await repo.GetProviderOpportunities(opportunityId);
            var tokensBuilder = new List<IDictionary<string, string>>();

            foreach (var referral in providers)
            {
                var toAddress = referral.ProviderPrimaryContactEmail;
                var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                var tokens = new Dictionary<string, string>
                {
                    { "opportunity_id", referral.OpportunityId.ToString()},
                    { "primary_contact_name", referral.ProviderPrimaryContact },
                    { "provider_name", referral.ProviderName },
                    { "route", referral.RouteName.ToLowerInvariant() },
                    { "venue_postcode", referral.ProviderVenuePostcode },
                    { "search_radius", referral.SearchRadius.ToString() },
                    { "job_role", referral.JobRole },
                    { "employer_business_name", referral.CompanyName },
                    { "employer_contact_name", referral.EmployerContact},
                    { "employer_contact_number", referral.EmployerContactPhone },
                    { "employer_contact_email", referral.EmployerContactEmail },
                    { "employer_postcode", $"{referral.Town} {referral.Postcode }" },
                    { "number_of_placements", placements }
                };

                tokensBuilder.Add(tokens);

            }

            return tokensBuilder;
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }
    }
}
