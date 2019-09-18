using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentValidation;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Event;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Created_Event_For_Existing_Employer
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;

        public When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Created_Event_For_Existing_Employer()
        {
            var mapper = Substitute.For<IMapper>();

            mapper.Map<Domain.Models.Employer>(Arg.Any<CrmEmployerEventBase>()).Returns(new Domain.Models.Employer());

            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new Domain.Models.Employer());

            var employerService = new EmployerService(_employerRepository, opportunityRepository, mapper, new CrmEmployerEventDataValidator(), Substitute.For<IValidator<CrmContactEventBase>>());

            var employerEventBase = CrmEmployerEventBaseBuilder.Buiild(true);

            var data = JsonConvert.SerializeObject(employerEventBase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            employerService.HandleEmployerCreatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_Be_Updated()
        {
            _employerRepository.Received(1).Update(Arg.Any<Domain.Models.Employer>());
            _employerRepository.DidNotReceive().Create(Arg.Any<Domain.Models.Employer>());
        }
    }
}