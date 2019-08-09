using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
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
            var emailHistoryService = Substitute.For<IEmailHistoryService>();
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
                            ? (object) new
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
                SendEmailEnabled = true,
                NotificationsSystemId = "TLevelsIndustryPlacement"
            };

            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            _opportunityItemRepository.GetMany(Arg.Any<Expression<Func<OpportunityItem, bool>>>())
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

            opportunityRepo.GetProviderOpportunities(Arg.Any<int>(), Arg.Any<IEnumerable<int>>())
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

            backgroundProcessHistoryRepo.GetSingleOrDefault(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>()).Returns(new BackgroundProcessHistory
            {
                Id = 1,
                Status = BackgroundProcessHistoryStatus.Pending.ToString()
            });

            var itemIds = new List<int>
            {
                1
            };

            var referralService = new ReferralEmailService(mapper, configuration, dateTimeProvider, emailService,
                emailHistoryService, opportunityRepo, _opportunityItemRepository, backgroundProcessHistoryRepo);

            referralService.SendProviderReferralEmailAsync(1, itemIds, 1,  httpcontextAccesor.HttpContext.User.GetUserName()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnly(Arg.Any<IList<OpportunityItem>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>()
                );
        }

        [Fact]
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Four_Items_With_Expected_Values()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnly(Arg.Is<IList<OpportunityItem>>(
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
    }
}