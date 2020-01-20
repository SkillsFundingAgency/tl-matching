using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.Provider
{
    public class When_Provider_BackFillProviderDisplayName_Function_Fires
    {
        [Fact]
        public void ProviderRepository_Results_Should_Have_Expected_Values()
        {
            var providers = new List<Domain.Models.Provider>
            {
                new Domain.Models.Provider
                {
                    //Id = 1,
                    Name = "War and Peace College",
                    DisplayName = null
                }
            }.AsQueryable();

            var mockSet = Substitute.For<DbSet<Domain.Models.Provider>, IAsyncEnumerable<Domain.Models.Provider>, IQueryable<Domain.Models.Provider>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<Domain.Models.Provider>)mockSet).GetEnumerator()
                .Returns(new FakeAsyncEnumerator<Domain.Models.Provider>(providers.GetEnumerator()));
            ((IQueryable<Domain.Models.Provider>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<Domain.Models.Provider>(providers.Provider));
            ((IQueryable<Domain.Models.Provider>)mockSet).Expression.Returns(providers.Expression);
            ((IQueryable<Domain.Models.Provider>)mockSet).ElementType.Returns(providers.ElementType);
            ((IQueryable<Domain.Models.Provider>)mockSet).GetEnumerator().Returns(providers.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions);
            mockContext.Set<Domain.Models.Provider>().Returns(mockSet);

            IRepository<Domain.Models.Provider> providerRepository = Substitute.For<GenericRepository<Domain.Models.Provider>>(
                NullLogger<GenericRepository<Domain.Models.Provider>>.Instance, mockContext);

            var providerFunctions = new Functions.Provider();

            providerFunctions.BackFillProviderDisplayNameAsync(
                new TimerInfo(new ConstantSchedule(TimeSpan.Zero), null),
                new ExecutionContext(), new NullLogger<Functions.Provider>(), providerRepository,
                Substitute.For<IRepository<FunctionLog>>()).GetAwaiter().GetResult();

            providerRepository.Received(1).UpdateManyAsync(Arg.Is<IList<Domain.Models.Provider>>(p => p.First().DisplayName == "War and Peace College"));
        }
    }
}