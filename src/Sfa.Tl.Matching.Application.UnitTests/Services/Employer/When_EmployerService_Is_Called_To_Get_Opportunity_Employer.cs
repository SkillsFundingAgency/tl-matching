﻿using System;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
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

            _opportunityRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, FindEmployerViewModel>>>())
                .Returns(new FindEmployerViewModelBuilder().BuildWithEmployer());

            var employerService = new EmployerService(employerRepository, _opportunityRepository);

            _result = employerService.GetOpportunityEmployerAsync(1, 2).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, FindEmployerViewModel>>>());
        }

        [Fact]
        public void Then_Result_Fields_Are_As_Expected()
        {
            _result.OpportunityItemId.Should().Be(1);
            _result.OpportunityId.Should().Be(2);
            _result.SelectedEmployerId.Should().Be(3);
            _result.CompanyName.Should().Be("CompanyName");
            _result.PreviousCompanyName.Should().Be("CompanyName");
        }
    }
}