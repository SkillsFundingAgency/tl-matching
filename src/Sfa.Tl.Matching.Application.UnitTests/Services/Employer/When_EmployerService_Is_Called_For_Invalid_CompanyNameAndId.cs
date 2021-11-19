using System;
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
    public class When_EmployerService_Is_Called_For_Invalid_CompanyNameAndId
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly bool _employerResult;

        private readonly Guid _employerCrmId = new("11111111-1111-1111-1111-111111111111");
        private const string CompanyName = "CompanyName";

        public When_EmployerService_Is_Called_For_Invalid_CompanyNameAndId()
        {
            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>(),
                    Arg.Any<Expression<Func<Domain.Models.Employer, Guid>>>())
                .Returns(Guid.Empty);

            var employerService = new EmployerService(_employerRepository, opportunityRepository, Substitute.For<IMapper>(), Substitute.For<IValidator<CrmEmployerEventBase>>(),
                Substitute.For<IMessageQueueService>());

            _employerResult = employerService.ValidateCompanyNameAndCrmIdAsync(_employerCrmId, CompanyName).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetEmployer_Is_Called_Exactly_Once()
        {
            _employerRepository.Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>(),
                                    Arg.Any<Expression<Func<Domain.Models.Employer, Guid>>>());
        }

        [Fact]
        public void Then_The_Employer_Validation_Result_Is_False()
        {
            _employerResult.Should().Be(false);
        }
    }
}