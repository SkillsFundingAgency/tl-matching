using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Confirm_Multiple_And_No_Remaining_Referrals_With_ProvisionGaps
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_OpportunityService_Is_Called_To_Confirm_Multiple_And_No_Remaining_Referrals_With_ProvisionGaps()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();
            httpcontextAccesor.HttpContext.Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, "TestUser")
                }))
            });

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));

            var backgroundProcessHistoryRepo = Substitute.For<IRepository<BackgroundProcessHistory>>();
            var emailService = Substitute.For<IEmailService>();
            var opportunityRepo = Substitute.For<IOpportunityRepository>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver")
                        ?
                        new LoggedInUserEmailResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(
                            httpcontextAccesor)
                        : type.Name.Contains("LoggedInUserNameResolver")
                            ? (object)new
                                LoggedInUserNameResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(
                                    httpcontextAccesor)
                            : type.Name.Contains("UtcNowResolver")
                                ? new UtcNowResolver<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>(
                                    dateTimeProvider)
                                :
                                null);
            });
            var mapper = new Mapper(config);

            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(new List<OpportunityItem>
                {
                    new OpportunityItem
                    {
                        Id = 1,
                        IsSaved = true,
                        IsSelectedForReferral = true,
                        OpportunityType = OpportunityType.Referral.ToString()
                    },
                    new OpportunityItem
                    {
                        Id = 2,
                        IsSaved = true,
                        IsSelectedForReferral = true,
                        OpportunityType = OpportunityType.Referral.ToString()
                    },
                    new OpportunityItem
                    {
                        Id = 3,
                        IsSaved = true,
                        IsSelectedForReferral = true,
                        OpportunityType = OpportunityType.Referral.ToString()
                    },
                    new OpportunityItem
                    {
                        Id = 4,
                        IsSaved = true,
                        IsSelectedForReferral = true,
                        OpportunityType = OpportunityType.ProvisionGap.ToString()
                    }
                }.AsQueryable());

            opportunityRepo.GetProviderOpportunitiesAsync(Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
                .Returns(new List<OpportunityReferralDto>
                {
                    new OpportunityReferralDto
                    {
                        OpportunityId = 1,
                        OpportunityItemId = 1,
                        ProviderPrimaryContact = "contact",
                        ProviderName = "Name",
                        RouteName = "Routename",
                        ProviderVenueTown = "Provider town",
                        ProviderVenuePostcode = "Provider postcode",
                        DistanceFromEmployer = "3.5",
                        JobRole = "Job role",
                        CompanyName = "Companyname",
                        EmployerContact = "Employer contact",
                        EmployerContactPhone = "Employer phone",
                        EmployerContactEmail = "Employer email",
                        Town = "Town",
                        Postcode =  "Postcode",
                        Placements =  1
                    }
                });

            backgroundProcessHistoryRepo.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>()).Returns(new BackgroundProcessHistory
            {
                Id = 1,
                Status = BackgroundProcessHistoryStatus.Pending.ToString()
            });

            var itemIds = new List<int>
            {
                1
            };

            var referralService = new ReferralEmailService(mapper, configuration, dateTimeProvider, emailService,
                opportunityRepo, _opportunityItemRepository, backgroundProcessHistoryRepo);

            referralService.SendProviderReferralEmailAsync(1, itemIds, 1, httpcontextAccesor.HttpContext.User.GetUserName()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnlyAsync(Arg.Any<IList<OpportunityItem>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>()
                );
        }

        [Fact]
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Four_Items_With_Expected_Values()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnlyAsync(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 4
                             && o[0].Id == 1
                             && o[0].IsCompleted
                             && o[1].Id == 2
                             && o[1].IsCompleted
                             && o[2].Id == 3
                             && o[2].IsCompleted
                             && o[3].Id == 4
                             && o[3].IsCompleted
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Theory, AutoDomainData]
        public async Task Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Two_Items_With_Expected_Values(
            MatchingConfiguration configuration,
            IDateTimeProvider dateTimeProvider,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue providerVenue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            [Frozen] IEmailHistoryService emailHistoryService,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<OpportunityRepository> oppLogger,
            ILogger<GenericRepository<BackgroundProcessHistory>> backgroundLogger,
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor
            )
        {
            //Arrange
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));
            httpContextAccessor.HttpContext = httpContext;

            var config =
                MapperConfig<OpportunityMapper, OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>
                    .Config(httpContextAccessor, dateTimeProvider);

            var mapper = new Mapper(config);

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, providerVenue, opportunity, backgroundProcessHistory);

            var opportunityRepository = new OpportunityRepository(oppLogger, dbContext);
            var opportunityItemRepository = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var backgroundProcessHistoryRepo = new GenericRepository<BackgroundProcessHistory>(backgroundLogger, dbContext);

            var referralService = new ReferralEmailService(mapper, configuration, dateTimeProvider, emailService,
                emailHistoryService, opportunityRepository, opportunityItemRepository, backgroundProcessHistoryRepo);

            var itemIds = opportunity.OpportunityItem.Select(oi => oi.Id).ToList();

            //Act
            await referralService.SendProviderReferralEmailAsync(opportunity.Id,
                itemIds, backgroundProcessHistory.Id,
                httpContext.User.GetUserName());

            //Assert
            var completedItems = dbContext.OpportunityItem.Where(oi => itemIds.Contains(oi.Id));

            completedItems.Select(x => x.IsCompleted).Should().Contain(true);

            completedItems.Where(x => x.IsCompleted).Select(x => x.ModifiedBy).Should()
                .Contain(httpContext.User.GetUserName());
        }
    }
}