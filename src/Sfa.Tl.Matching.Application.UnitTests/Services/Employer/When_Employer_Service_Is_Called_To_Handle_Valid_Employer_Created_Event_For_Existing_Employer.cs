﻿using System;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
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
    public class When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Created_Event_For_Existing_Employer
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly CrmEmployerEventBase _employerEventBase;

        public When_Employer_Service_Is_Called_To_Handle_Valid_Employer_Created_Event_For_Existing_Employer()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns(new Domain.Models.Employer());

            _messageQueueService = Substitute.For<IMessageQueueService>();
            var employerService = new EmployerService(_employerRepository, opportunityRepository, mapper, new CrmEmployerEventDataValidator(),
                _messageQueueService);

            _employerEventBase = new CrmEmployerEventBaseBuilder()
                .WithValidAupaStatus().Build();

            var data = JsonSerializer.Serialize(_employerEventBase,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                });

            employerService.HandleEmployerCreatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_Be_Updated()
        {
            _employerRepository.DidNotReceive().CreateAsync(Arg.Any<Domain.Models.Employer>());

            _employerRepository.Received(1).UpdateAsync(Arg.Is<Domain.Models.Employer>(e =>
                e.CrmId == _employerEventBase.AccountId.ToGuid() &&
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