using System;
using System.Linq.Expressions;
using AutoMapper;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Event;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Updated_Event_For_New_Employer
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly CrmEmployerEventBase _employerEventBase;

        public When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Updated_Event_For_New_Employer()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);


            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns((Domain.Models.Employer)null);

            _messageQueueService = Substitute.For<IMessageQueueService>();
            var employerService = new EmployerService(_employerRepository, opportunityRepository, mapper, new CrmEmployerEventDataValidator(),
                _messageQueueService);

            _employerEventBase = CrmEmployerEventBaseBuilder.Buiild(true);

            var data = JsonConvert.SerializeObject(_employerEventBase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            employerService.HandleEmployerUpdatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_Be_Created()
        {
            _employerRepository.DidNotReceive().Update(Arg.Any<Domain.Models.Employer>());
            _employerRepository.Received(1).Create(Arg.Is<Domain.Models.Employer>(e =>
                e.CrmId == _employerEventBase.accountid.ToGuid() &&
                e.Aupa == "Aware" &&
                e.CompanyName == "Test" &&
                e.AlsoKnownAs == "Test" &&
                e.CompanyNameSearch == "TestTest" &&
                e.PrimaryContact == "Test" &&
                e.Email == "Test@test.com" &&
                e.Phone == "0123456789" &&
                e.Owner == "Test"
                ));
            _messageQueueService.DidNotReceive()
                .PushEmployerAupaBlankEmailMessageAsync(Arg.Any<SendEmployerAupaBlankEmail>());
        }
    }
}