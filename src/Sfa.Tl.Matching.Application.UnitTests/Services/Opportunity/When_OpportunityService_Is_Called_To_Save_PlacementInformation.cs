using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Save_PlacementInformation
    {
        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;
        private const string JobTitle = "JobTitle";
        private const bool PlacementsKnown = true;
        private const int Placements = 5;
        private const string ModifiedBy = "ModifiedBy";
        private const int OpportunityId = 1;
        private const string Postcode = "ModifiedBy";
        private const int Distance = 1;
        private const int RouteId = 1;

        public When_OpportunityService_Is_Called_To_Save_PlacementInformation()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var provisionGapRepository = Substitute.For<IRepository<Domain.Models.ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            var opportunity = new Domain.Models.Opportunity { Id = OpportunityId, Postcode = Postcode, Distance = Distance, RouteId = RouteId };

            _opportunityRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>()).Returns(opportunity);

            var opportunityService = new OpportunityService(mapper, dateTimeProvider, _opportunityRepository, provisionGapRepository, referralRepository);

            var dto = new PlacementInformationSaveViewModel
            {
                OpportunityId = OpportunityId,
                JobTitle = JobTitle,
                PlacementsKnown = PlacementsKnown,
                Placements = Placements,
                ModifiedBy = ModifiedBy
            };

            opportunityService.SavePlacementInformation(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Update_Is_Called_Exactly_Once_With_1_Placement()
        {
            _opportunityRepository.Received(1).Update(Arg.Is<Domain.Models.Opportunity>(opportunity => 
                opportunity.Id == OpportunityId &&
                opportunity.JobTitle == JobTitle &&
                opportunity.PlacementsKnown == PlacementsKnown &&
                opportunity.Placements == Placements &&
                opportunity.ModifiedBy == ModifiedBy &&
                opportunity.Postcode == Postcode &&
                opportunity.Distance == Distance &&
                opportunity.RouteId == Distance
                ));
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }
    }
}