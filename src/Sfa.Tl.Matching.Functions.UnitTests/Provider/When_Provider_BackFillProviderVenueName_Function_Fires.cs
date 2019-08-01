using System;
using System.Collections.Generic;
using System.Linq;
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
    public class When_Provider_BackFillProviderVenueName_Function_Fires
    {
        private readonly IList<ProviderVenue> _results;

        public When_Provider_BackFillProviderVenueName_Function_Fires()
        {
            var logger = Substitute.For<ILogger<GenericRepository<ProviderVenue>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new List<ProviderVenue>
                {
                    new ProviderVenue
                    {
                        Postcode = "CV1 2WT",
                        Name = null
                    },
                    new ProviderVenue
                    {
                        Postcode = "CV2 1AA",
                        Name = "Venue Name"
                    }
                });

                dbContext.SaveChanges();

                IRepository<ProviderVenue> providerVenueRepository = new GenericRepository<ProviderVenue>(logger, dbContext);

                var providerFunctions = new Functions.Provider();

                providerFunctions.BackFillProviderVenueName(new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null),
                    new ExecutionContext(), new NullLogger<Functions.Provider>(), providerVenueRepository,
                    Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();

                _results = dbContext.ProviderVenue.ToList();
            }
        }

        [Fact]
        public void ProviderVenueRepository_Results_Should_Have_Expected_Values()
        {
            _results.Count.Should().Be(2);
            _results[0].Name.Should().Be("CV1 2WT");
            _results[1].Name.Should().Be("Venue Name");
        }
    }
}