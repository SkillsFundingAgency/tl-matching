using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class EmployerRepository : IRepository<Employer>
    {
        private readonly ILogger<EmployerRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public EmployerRepository(ILogger<EmployerRepository> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> CreateMany(IEnumerable<Employer> employers)
        {
            await ResetData();

            _dbContext.Employer.AddRange(employers);

            int createdRecordsCount;
            try
            {
                createdRecordsCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
            
            return createdRecordsCount;
        }

        public Task<Employer> GetSingleOrDefault(Func<Employer, bool> predicate)
        {
            return _dbContext.Employer.SingleOrDefaultAsync(employer => predicate(employer));
        }

        public async Task ResetData()
        {
            try
            {
                await _dbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.Employer");
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e.InnerException);
                throw;
            }
        }
    }
}