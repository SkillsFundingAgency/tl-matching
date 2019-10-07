using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Clear_Opportunity_Items_Selected_For_Referral
    {
        
        [Theory, AutoDomainData]
        public async Task When_OpportunityService_Is_Called_To_Clear_Items_Then_UpdateManyWithSpecifedColumnsOnly_Is_Called_With_Two_Items_With_Expected_Values(
            IDateTimeProvider dateTimeProvider,
            Domain.Models.Provider provider,
            Domain.Models.ProviderVenue providerVenue,
            Domain.Models.Opportunity opportunity,
            BackgroundProcessHistory backgroundProcessHistory,
            IOpportunityRepository opportunityRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Domain.Models.Referral> referralRepository,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            MatchingDbContext dbContext,
            HttpContext httpContext,
            HttpContextAccessor httpContextAccessor
            )
        {
            //Arrange
            dateTimeProvider.UtcNow().Returns(new DateTime(2019, 1, 1));
            httpContextAccessor.HttpContext = httpContext;

            var config = MapperConfig<OpportunityMapper, OpportunityItemIsSelectedForReferralDto, OpportunityItem>.Config(httpContextAccessor, dateTimeProvider);

            var mapper = new Mapper(config);

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();

            await ReferralsInMemoryTestData.SetTestData(dbContext, provider, providerVenue, opportunity,
                backgroundProcessHistory);

            var opportunityItemRepository = new GenericRepository<OpportunityItem>(itemLogger, dbContext);

            var opportunityService = new OpportunityService(mapper, opportunityRepository, opportunityItemRepository,
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            //Act
            await opportunityService.ClearOpportunityItemsSelectedForReferralAsync(opportunity.Id);

            //Assert
            var item = dbContext.OpportunityItem.FirstOrDefault(oi => oi.OpportunityId == opportunity.Id);

            dbContext.OpportunityItem.Count(oi => oi.OpportunityId == opportunity.Id).Should().Be(2);
            item?.ModifiedBy.Should().Be(httpContext.User.GetUserName());
            item?.IsSelectedForReferral.Should().BeFalse();
            item?.ModifiedOn.Should().Be(new DateTime(2019, 1, 1));
        }
    }


}