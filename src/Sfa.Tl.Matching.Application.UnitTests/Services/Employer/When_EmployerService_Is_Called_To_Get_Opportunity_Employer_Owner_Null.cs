using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_EmployerService_Is_Called_To_Get_Opportunity_Employer_Owner_Null
    {
        private readonly string _result;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_EmployerService_Is_Called_To_Get_Opportunity_Employer_Owner_Null()
        {
            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            _opportunityRepository.GetFirstOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>())
                .Returns((Domain.Models.Opportunity)null);

            var employerService = new EmployerService(employerRepository, _opportunityRepository);

            _result = employerService.GetEmployerOpportunityLockedByOwnerAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetFirstOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetFirstOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }

        [Fact]
        public void Then_Result_Is_Null_Or_Empty()
        {
            _result.Should().BeNullOrEmpty();
            _result.Should().BeNull();
        }
    }
}