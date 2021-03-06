﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Event;
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

            _opportunityRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>())
                .Returns((Domain.Models.Opportunity)null);

            var employerService = new EmployerService(employerRepository, _opportunityRepository, Substitute.For<IMapper>(), Substitute.For<IValidator<CrmEmployerEventBase>>(),
                Substitute.For<IMessageQueueService>());

            _result = employerService.GetEmployerOpportunityOwnerAsync(new Guid("11111111-1111-1111-1111-111111111111")).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetFirstOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }

        [Fact]
        public void Then_Result_Is_Null_Or_Empty()
        {
            _result.Should().BeNullOrEmpty();
            _result.Should().BeNull();
        }
    }
}