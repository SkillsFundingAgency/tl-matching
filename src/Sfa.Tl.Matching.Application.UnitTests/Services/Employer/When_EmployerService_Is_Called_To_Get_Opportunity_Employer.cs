using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Event;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_EmployerService_Is_Called_To_Get_Opportunity_Employer
    {
        private readonly FindEmployerViewModel _result;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_EmployerService_Is_Called_To_Get_Opportunity_Employer()
        {
            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            _opportunityRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, FindEmployerViewModel>>>())
                .Returns(new FindEmployerViewModelBuilder().BuildWithEmployer());

            var employerService = new EmployerService(employerRepository, _opportunityRepository, Substitute.For<IMapper>(), Substitute.For<IValidator<CrmEmployerEventBase>>(),
                Substitute.For<IMessageQueueService>());

            _result = employerService.GetOpportunityEmployerAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, FindEmployerViewModel>>>());
        }

        [Fact]
        public void Then_Result_Fields_Are_As_Expected()
        {
            _result.OpportunityItemId.Should().Be(1);
            _result.OpportunityId.Should().Be(2);
            _result.SelectedEmployerCrmId.Should().Be(new Guid("33333333-3333-3333-3333-333333333333"));
            _result.CompanyName.Should().Be("CompanyName");
            _result.PreviousCompanyName.Should().Be("CompanyName");
        }
    }
}