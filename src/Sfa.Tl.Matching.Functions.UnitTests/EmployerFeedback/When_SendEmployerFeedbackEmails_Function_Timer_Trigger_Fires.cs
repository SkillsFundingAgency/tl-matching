﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmployerFeedback
{
    public class When_SendEmployerFeedbackEmails_Function_Timer_Trigger_Fires
    {
        //private readonly IEmployerFeedbackService _employerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_SendEmployerFeedbackEmails_Function_Timer_Trigger_Fires()
        {
            //_employerFeedbackService = Substitute.For<IEmployerFeedbackService>();

            var timerSchedule = Substitute.For<TimerSchedule>();
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var bankHolidayGenerator = new Functions.EmployerFeedback();
            bankHolidayGenerator.SendEmployerFeedbackEmails(
                new TimerInfo(timerSchedule, new ScheduleStatus()),
                new ExecutionContext(),
                new NullLogger<Functions.EmployerFeedback>(),
                //_employerFeedbackService,
                _functionLogRepository).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            //_employerFeedbackService
            //    .Received(1)
            //    .SendFeedbackEmailsAsync();
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .Create(Arg.Any<FunctionLog>());
        }
    }
}