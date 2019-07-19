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
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Opportunity_Basket_ReferralMultipleAndProvisionGap
    {
        private readonly OpportunityBasketViewModel _result;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_OpportunityService_Is_Called_To_Get_Opportunity_Basket_ReferralMultipleAndProvisionGap()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var opportunityPipelineReportWriter = Substitute.For<IFileWriter<OpportunityPipelineDto>>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();

            var viewModel = new OpportunityBasketViewModelBuilder()
                .AddReferralItem()
                .AddReferralItem()
                .AddReferralItem()
                .AddProvisionGapItem()
                .Build();

            _opportunityRepository.GetOpportunityBasket(1).Returns(viewModel);

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, 
                provisionGapRepository, referralRepository, googleMapApiClient,
                opportunityPipelineReportWriter, dateTimeProvider);

            _result = opportunityService.GetOpportunityBasket(1)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunityBasket_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetOpportunityBasket(1);
        }

        [Fact]
        public void Then_ViewModel_Is_Correct()
        {
            _result.Type.Should().Be(OpportunityBasketType.MultipleReferralAndProvisionGap);
            _result.CompanyName.Should().Be("CompanyName");
            _result.CompanyNameAka.Should().Be("AlsoKnownAs");
            _result.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");
        }
    }
}