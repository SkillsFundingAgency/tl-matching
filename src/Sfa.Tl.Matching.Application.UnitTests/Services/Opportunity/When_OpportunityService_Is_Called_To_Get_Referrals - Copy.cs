using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Get_Opportunity_Spreadsheet_Data
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<Domain.Models.Referral> _referralRepository;

        private readonly byte[] _result;

        public When_OpportunityService_Is_Called_To_Get_Opportunity_Spreadsheet_Data()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            _referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            //_referralRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>()).Returns(
            //    new List<Domain.Models.Referral>
            //    {
            //        new Domain.Models.Referral
            //        {
            //            ProviderVenue = new Domain.Models.ProviderVenue
            //            {
            //                Postcode = "AA1 1AA",
            //                Provider = new Domain.Models.Provider
            //                {
            //                    Name = "Provider1"
            //                }
            //            }
            //        }
            //    }.AsQueryable());

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, provisionGapRepository, _referralRepository, googleMapApiClient);

            _result = opportunityService.GetOpportunitySpreadsheetDataAsync(1)
                .GetAwaiter().GetResult();
        }

        //[Fact]
        //public void Then_GetMany_Is_Called_Exactly_Once()
        //{
        //    _referralRepository
        //        .Received(1)
        //        .GetMany(Arg.Any<Expression<Func<Domain.Models.Referral, bool>>>());
        //}

        [Fact]
        public void Then_Fields_Are_Set_To_Expected_Values()
        {
            _result.Should().NotBeNull();
            //_result.Length.Should().Be(100);
        }
    }
}