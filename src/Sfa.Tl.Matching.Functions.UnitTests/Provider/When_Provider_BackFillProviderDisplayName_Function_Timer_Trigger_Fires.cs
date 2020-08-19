using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Provider
{
    public class When_Provider_BackFillProviderDisplayName_Function_Timer_Trigger_Fires
    {
        private readonly IList<Domain.Models.Provider> _results;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_Provider_BackFillProviderDisplayName_Function_Timer_Trigger_Fires()
        {
            var logger = Substitute.For<ILogger<ProviderRepository>>();

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new List<Domain.Models.Provider>
            {
                new Domain.Models.Provider
                {
                    Id = 1,
                    Name = "War and Peace College",
                    DisplayName = null
                },
                new Domain.Models.Provider
                {
                    Id = 2,
                    Name = "Provider 2",
                    DisplayName = "Display Name"
                }
            });

            dbContext.SaveChanges();

            IRepository<Domain.Models.Provider> providerRepository = new ProviderRepository(logger, dbContext);
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var providerFunctions = new Functions.Provider(providerRepository, _functionLogRepository);

            providerFunctions.BackFillProviderDisplayNameAsync(
                new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null),
                new ExecutionContext(), 
                new NullLogger<Functions.Provider>()
                ).GetAwaiter().GetResult();

            _results = dbContext.Provider.ToList();
        }

        [Fact]
        public void ProviderRepository_Results_Should_Have_Expected_Values()
        {
            _results.Count.Should().Be(2);
            _results.Single(p => p.Id == 1).DisplayName.Should().Be("War and Peace College");
            _results.Single(p => p.Id == 2).DisplayName.Should().Be("Display Name");
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