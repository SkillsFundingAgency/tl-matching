using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_EmployerService_Is_Called_For_Invalid_EmployerNameAndId
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly bool _employerResult;

        private const int EmployerId = 1;
        private const string EmployerName = "EmployerName";

        public When_EmployerService_Is_Called_For_Invalid_EmployerNameAndId()
        {
            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Employer, int>>>())
                .Returns(0);

            var employerService = new EmployerService(_employerRepository, opportunityRepository);

            _employerResult = employerService.ValidateEmployerNameAndId(EmployerId, EmployerName).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>(),
                                    Arg.Any<Expression<Func<Domain.Models.Employer, int>>>());
        }

        [Fact]
        public void Then_The_Employer_Validation_Result_Is_False()
        {
            _employerResult.Should().Be(false);
        }
    }
}