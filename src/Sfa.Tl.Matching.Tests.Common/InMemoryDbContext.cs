using System;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data;

namespace Sfa.Tl.Matching.Tests.Common
{
    public static class InMemoryDbContext
    {
        public static MatchingDbContext Create()
        {
            var options = new DbContextOptionsBuilder<MatchingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            var context = new MatchingDbContext(options);

            return context;
        }
    }
}
