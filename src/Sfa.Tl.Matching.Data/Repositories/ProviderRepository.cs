﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class ProviderRepository : AbstractBaseRepository<Provider>
    {
        private readonly MatchingDbContext _dbContext;

        public ProviderRepository(ILogger<ProviderRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IEnumerable<Provider> providers)
        {
            return await BaseCreateMany(providers);
        }

        public override async Task<Provider> GetSingleOrDefault(Func<Provider, bool> predicate)
        {
            return await _dbContext.Provider.SingleOrDefaultAsync(p => predicate(p));
        }
    }
}