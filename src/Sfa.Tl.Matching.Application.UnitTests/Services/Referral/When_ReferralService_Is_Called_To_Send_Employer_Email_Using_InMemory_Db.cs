using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class When_ReferralService_Is_Called_To_Send_Employer_Email_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Employers(
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
            var service = new ReferralService(config, emailService, emailHistoryService, repo);

            //Act
            await service.SendEmployerReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
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
            var service = new ReferralService(config, emailService, emailHistoryService, repo);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id);

            //Act
            await service.SendEmployerReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic => dic.ContainsKey("placements_list")), Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic => dic.ContainsKey("placements_list") && dic["placements_list"] == expectedResult), Arg.Any<string>());

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
            var service = new ReferralService(config, emailService, emailHistoryService, repo);

            //Act
            await service.SendEmployerReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("employer_contact_name") && dic["employer_contact_name"] == opportunity.EmployerContact),
                Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("employer_business_name") && dic["employer_business_name"] == opportunity.Employer.CompanyName),
                Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("employer_contact_number") && dic["employer_contact_number"] == opportunity.EmployerContactPhone),
                Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("employer_contact_email") && dic["employer_contact_email"] == opportunity.EmployerContactEmail),
                Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("employer_postcode") && dic["employer_postcode"] == opportunity.Employer.Postcode),
                Arg.Any<string>());


        }

        [Theory, AutoDomainData]
        public async Task Then_Placements_List_Should_Be_Empty_If_Opp_Items_Not_Saved(
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
            await SetTestData(dbContext, provider, venue, opportunity, false, false);

            var repo = new OpportunityRepository(logger, dbContext);
            var service = new ReferralService(config, emailService, emailHistoryService, repo);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id);

            //Act
            await service.SendEmployerReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("placements_list") && dic["placements_list"] == string.Empty), Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("placements_list") && dic["placements_list"] == expectedResult), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Placements_List_Should_Be_Empty_If_Not_Selected_For_Referral(
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
            await SetTestData(dbContext, provider, venue, opportunity, true, false);

            var repo = new OpportunityRepository(logger, dbContext);
            var service = new ReferralService(config, emailService, emailHistoryService, repo);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id);

            //Act
            await service.SendEmployerReferralEmail(opportunity.Id);

            //Assert
            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("placements_list") && dic["placements_list"] == string.Empty), Arg.Any<string>());

            await emailService.Received(1).SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(dic =>
                    dic.ContainsKey("placements_list") && dic["placements_list"] == expectedResult), Arg.Any<string>());
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
                opportunityItem.IsSelectedForReferral = isSelectedForReferral;
                
                dbContext.Entry(opportunityItem).Property("IsSaved").IsModified = true;
                dbContext.Entry(opportunityItem).Property("IsSelectedForReferral").IsModified = true;
            }

            await dbContext.SaveChangesAsync();
        }

        private static async Task<string> GetExpectedResult(OpportunityRepository repo, int opportunityId)
        {
            var employerReferral = await repo.GetEmployerReferrals(opportunityId);
            var sb = new StringBuilder();

            foreach (var data in employerReferral.ProviderReferrals)
            {
                var placements = GetNumberOfPlacements(data.PlacementsKnown, data.Placements);

                sb.AppendLine($"# {data.ProviderVenueTown} {data.ProviderVenuePostCode}");
                sb.AppendLine($"*Job role: {data.JobRole}");
                sb.AppendLine($"*Students wanted: {placements}");
                sb.AppendLine($"*Providers selected: {data.ProviderName}");
                sb.AppendLine("");
            }

            return sb.ToString();
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }
    }
}
