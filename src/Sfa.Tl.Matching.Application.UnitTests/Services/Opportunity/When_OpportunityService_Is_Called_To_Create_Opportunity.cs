using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity
    {
        private readonly int _result;
        private const int OpportunityId = 1;

        public When_OpportunityService_Is_Called_To_Create_Opportunity()
        {
            var config = new MapperConfiguration(c => c.AddProfile<OpportunityMapper>());
            var mapper = new Mapper(config);
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var repository = Substitute.For<IRepository<Domain.Models.Opportunity>>();

            repository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);
            
            var opportuntiyService = new OpportunityService(mapper, dateTimeProvider, repository);

            var dto = new OpportunityDto
            {
                EmployerContact = "Contact"
            };

            _result = opportuntiyService.CreateOpportunity(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(OpportunityId);
        }
    }
}