using System;
using System.Linq.Expressions;
using AutoMapper;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_Employer_Service_Is_Called_To_Handle_Invalid_Aupa_Null_Contact_Updated_Event
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;

        public When_Employer_Service_Is_Called_To_Handle_Invalid_Aupa_Null_Contact_Updated_Event()
        {
            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new Domain.Models.Employer());

            var employerService = new EmployerService(_employerRepository, opportunityRepository, Substitute.For<IMapper>(), new CrmEmployerEventDataValidator(),
                Substitute.For<IMessageQueueService>());

            var employerEventBase = new CrmEmployerEventBaseBuilder()
                .WithNullAupaStatus().Build();

            var data = JsonConvert.SerializeObject(employerEventBase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            employerService.HandleContactUpdatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_NOT_Be_Created_OR_Updated()
        {
            _employerRepository.DidNotReceive().CreateAsync(Arg.Any<Domain.Models.Employer>());
            _employerRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.Models.Employer>());
        }
    }
}