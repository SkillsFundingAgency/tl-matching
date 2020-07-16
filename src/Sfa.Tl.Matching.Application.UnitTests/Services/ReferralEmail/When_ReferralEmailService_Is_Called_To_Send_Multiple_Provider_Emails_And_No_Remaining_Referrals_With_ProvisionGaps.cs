using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail
{
    public class When_ReferralEmailService_Is_Called_To_Send_Multiple_Provider_Emails_And_No_Remaining_Referrals_With_ProvisionGaps
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_ReferralEmailService_Is_Called_To_Send_Multiple_Provider_Emails_And_No_Remaining_Referrals_With_ProvisionGaps()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
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
                            httpContextAccessor)
                        : type.Name.Contains("LoggedInUserNameResolver")
                            ? (object)new
                                LoggedInUserNameResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(
                                    httpContextAccessor)
                            : type.Name.Contains("UtcNowResolver")
                                ? new UtcNowResolver<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>(
                                    dateTimeProvider)
                                :
                                null);
            });
            var mapper = new Mapper(config);

            var opportunityItems = new List<OpportunityItem>
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
            };

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.GetManyAsync(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
                .Returns(opportunityItems
                    .AsQueryable());

            opportunityRepo.GetProviderReferralsAsync(Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
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
                    },
                    new OpportunityReferralDto
                    {
                        OpportunityId = 1,
                        OpportunityItemId = 2,
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
                    },
                    new OpportunityReferralDto
                    {
                        OpportunityId = 1,
                        OpportunityItemId = 3,
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
                    },
                    new OpportunityReferralDto
                    {
                        OpportunityId = 1,
                        OpportunityItemId = 4,
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

            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var itemIds = opportunityItems
                .Where(oi => oi.IsSaved && oi.IsSelectedForReferral)
                .Select(oi => oi.Id)
                .ToList();

            var referralService = new ReferralEmailService(mapper, dateTimeProvider, emailService,
                opportunityRepo, _opportunityItemRepository, backgroundProcessHistoryRepo, functionLogRepository);

            referralService.SendProviderReferralEmailAsync(1, itemIds, 1, httpContextAccessor.HttpContext.User.GetUserName()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateManyWithSpecifiedColumnsOnly_Is_Called_Exactly_Four_Times_With_Expected_Values()
        {
            _opportunityItemRepository.Received(4)
                .UpdateManyWithSpecifiedColumnsOnlyAsync(Arg.Any<IList<OpportunityItem>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Fact]
        public void Then_UpdateManyWithSpecifiedColumnsOnly_Is_Called_Exactly_Once_With_Opportunity_Item_One()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifiedColumnsOnlyAsync(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 1
                             && o[0].Id == 1
                             && o[0].IsCompleted
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Fact]
        public void Then_UpdateManyWithSpecifiedColumnsOnly_Is_Called_Exactly_Once_With_Opportunity_Item_Two()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifiedColumnsOnlyAsync(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 1
                             && o[0].Id == 2
                             && o[0].IsCompleted
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Fact]
        public void Then_UpdateManyWithSpecifiedColumnsOnly_Is_Called_Exactly_Once_With_Opportunity_Item_Three()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifiedColumnsOnlyAsync(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 1
                             && o[0].Id == 3
                             && o[0].IsCompleted
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }

        [Fact]
        public void Then_UpdateManyWithSpecifiedColumnsOnly_Is_Called_Exactly_Once_With_Opportunity_Item_Four()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifiedColumnsOnlyAsync(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 1
                             && o[0].Id == 4
                             && o[0].IsCompleted
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }
    }
}