using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmployerAupaBlankEmail
{
    public class When_Employer_Aupa_Blank_Email_Function_Queue_Trigger_Fires
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        private const string EmployerName = "Test Employer";
        private const string EmployerOwner = "Test Owner";

        private const string SupportEmailAddress = "support@service.com";
        private readonly Guid _employerCrmId = new("8C8E2207-7137-4E60-AF60-1202123F05C9");

        public When_Employer_Aupa_Blank_Email_Function_Queue_Trigger_Fires()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                MatchingServiceSupportEmailAddress = SupportEmailAddress
            };

            _emailService = Substitute.For<IEmailService>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var employerAupaBlankEmail = new SendEmployerAupaBlankEmail
            {
                CrmId = _employerCrmId,
                Name = EmployerName,
                Owner = EmployerOwner
            };

            var employerAupaBlankEmailFunctions = new Functions.EmployerAupaBlankEmail(configuration, _emailService, _functionLogRepository);
            employerAupaBlankEmailFunctions.SendEmployerAupaBlankEmailAsync(
                employerAupaBlankEmail,
                new ExecutionContext(),
                new NullLogger<Functions.EmployerAupaBlankEmail>()
            ).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void SendEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int?>(), Arg.Any<int?>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void SendEmailAsync_Is_Called_Exactly_Once_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Is<string>(templateName => templateName == "EmployerAupaBlank"),
                    Arg.Is<string>(toAddress => toAddress == SupportEmailAddress),
                    Arg.Any<int?>(), Arg.Any<int?>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(s => s == "System"));
        }

        [Fact]
        public void SendEmailAsync_Is_Called_Exactly_Once_With_Expected_Tokens()
        {
            _emailService
                .Received(1)
                .SendEmailAsync(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<int?>(), Arg.Any<int?>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => tokens.ContainsKey("crm_id")
                                  && tokens["crm_id"] == _employerCrmId.ToString()
                                  && tokens.ContainsKey("employer_business_name")
                                  && tokens["employer_business_name"] == EmployerName
                                  && tokens.ContainsKey("employer_owner")
                                  && tokens["employer_owner"] == EmployerOwner),
                    Arg.Any<string>());
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}
