using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class OpportunityRepository : AbstractBaseRepository<Opportunity>
    {
        private readonly ILogger<OpportunityRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public OpportunityRepository(ILogger<OpportunityRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override async Task<int> Create(Opportunity opportunity)
        {
            return await BaseCreate(opportunity);
        }

        public override async Task<int> CreateMany(IEnumerable<Opportunity> opportunities)
        {
            return await BaseCreateMany(opportunities);
        }

        public override Task<IQueryable<Opportunity>> GetMany(Func<Opportunity, bool> predicate)
        {
            return Task.FromResult(_dbContext.Opportunity.Where(opportunity => predicate(opportunity)));
        }

        public override async Task<Opportunity> GetSingleOrDefault(Func<Opportunity, bool> predicate)
        {
            return await _dbContext.Opportunity.AsNoTracking().SingleOrDefaultAsync(o => predicate(o));
        }

        public override async Task Update(Opportunity opportunity)
        {
            await BaseUpdate(opportunity);
        }
    }
}