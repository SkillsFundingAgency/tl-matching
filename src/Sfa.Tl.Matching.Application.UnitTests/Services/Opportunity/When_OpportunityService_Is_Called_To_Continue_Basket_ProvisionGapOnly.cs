using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Continue_Basket_ProvisionGapOnly
    {
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_OpportunityService_Is_Called_To_Continue_Basket_ProvisionGapOnly()
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

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(OpportunityMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>(dateTimeProvider) :
                                null);
            });
            var mapper = new Mapper(config);
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();

            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            var viewModel = new ContinueOpportunityViewModel
            {
                SelectedOpportunity = new List<SelectedOpportunityItemViewModel>
                {
                    new SelectedOpportunityItemViewModel
                    {
                        Id = 1,
                        IsSelected = true,
                        OpportunityType = OpportunityType.ProvisionGap.ToString()
                    },
                    new SelectedOpportunityItemViewModel
                    {
                        Id = 2,
                        IsSelected = true,
                        OpportunityType = OpportunityType.ProvisionGap.ToString()
                    }
                }
            };

            opportunityService.ContinueWithOpportunities(viewModel).GetAwaiter().GetResult();
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
        public void Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Two_Items_With_Expected_Values()
        {
            _opportunityItemRepository.Received(1)
                .UpdateManyWithSpecifedColumnsOnly(Arg.Is<IList<OpportunityItem>>(
                        o => o.Count == 2
                             && o[0].Id == 1
                             && o[1].Id == 2
                             && o.All(x => x.IsCompleted)
                             && o.All(x => x.ModifiedBy == "TestUser")
                             && o.All(x => x.ModifiedOn == new DateTime(2019, 1, 1))
                    ),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, object>>>());
        }
    }
}