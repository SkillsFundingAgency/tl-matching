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
using Sfa.Tl.Matching.Models.Command;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_Employer_Service_Is_Called_To_Handle_Invalid_Aupa_Zero_Employer_Updated_Event
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly IMessageQueueService _messageQueueService;

        public When_Employer_Service_Is_Called_To_Handle_Invalid_Aupa_Zero_Employer_Updated_Event()
        {
            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            _employerRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new Domain.Models.Employer());

            var opportunityRepository = Substitute.For<IOpportunityRepository>();
            opportunityRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>())
                .Returns(new Domain.Models.Opportunity());

            _messageQueueService = Substitute.For<IMessageQueueService>();
            var employerService = new EmployerService(_employerRepository, opportunityRepository, Substitute.For<IMapper>(), new CrmEmployerEventDataValidator(),
                _messageQueueService);

            var employerEventBase = new CrmEmployerEventBaseBuilder()
                .WithZeroAupaStatus().Build();

            var data = JsonConvert.SerializeObject(employerEventBase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });
            
            employerService.HandleEmployerUpdatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_NOT_Be_Created_OR_Updated()
        {
            _employerRepository.DidNotReceive().CreateAsync(Arg.Any<Domain.Models.Employer>());
            _employerRepository.DidNotReceive().UpdateAsync(Arg.Any<Domain.Models.Employer>());

            _messageQueueService.Received(1)
                .PushEmployerAupaBlankEmailMessageAsync(Arg.Any<SendEmployerAupaBlankEmail>());
        }
    }
}