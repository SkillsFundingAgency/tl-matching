using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Confirm_Delete_Opportunity
    {
        private readonly ConfirmDeleteOpportunityItemViewModel _result;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_OpportunityService_Is_Called_To_Get_Confirm_Delete_Opportunity()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityReportDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var dto = new ValidConfirmDeleteOpportunityItemViewModelBuilder().Build();

            _opportunityItemRepository.GetSingleOrDefaultAsync(
                    Arg.Any<Expression<Func<OpportunityItem, bool>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, ConfirmDeleteOpportunityItemViewModel>>>())
                .Returns(dto);
            
            var opportunityService = new OpportunityService(mapper, opportunityRepository, _opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            _result = opportunityService.GetConfirmDeleteOpportunityItemAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityItemRepository
                .Received(1)
                .GetSingleOrDefaultAsync(
                    Arg.Any<Expression<Func<OpportunityItem, bool>>>(),
                    Arg.Any<Expression<Func<OpportunityItem, ConfirmDeleteOpportunityItemViewModel>>>());
        }

        [Fact]
        public void Then_Result_Fields_Are_Correct()
        {
            _result.OpportunityItemId.Should().Be(2);
            _result.OpportunityId.Should().Be(1);
            _result.CompanyName.Should().Be("CompanyName");
            _result.CompanyNameAka.Should().Be("AlsoKnownAs");
            _result.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");
            _result.Postcode.Should().Be("Postcode");
            _result.JobRole.Should().Be("JobRole");
            _result.BasketItemCount.Should().Be(1);
            _result.Placements.Should().Be(1);
        }
    }
}