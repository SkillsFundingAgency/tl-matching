using System;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Tl.Matching.Data.UnitTests
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
