using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_ProvisionGap
    {
        private readonly int _result;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private const int Id = 1;

        public When_OpportunityService_Is_Called_To_Create_ProvisionGap()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            _provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();

            _provisionGapRepository.Create(Arg.Any<ProvisionGap>()).Returns(Id);

            var opportunityService = new OpportunityService(mapper, dateTimeProvider, opportunityRepository, _provisionGapRepository);

            var dto = new CheckAnswersViewModel
            {
                OpportunityId = 1,
                ConfirmationSelected = true,
                CreatedBy = "Test"
            };

            _result = opportunityService.CreateProvisionGap(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Create_Is_Called_Exactly_Once()
        {
            _provisionGapRepository.Received(1).Create(Arg.Is<ProvisionGap>(p => p.OpportunityId == 1 && p.ConfirmationSelected == true && p.CreatedBy == "Test"));
        }

        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(Id);
        }
    }
}