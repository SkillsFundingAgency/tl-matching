using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class EmployerRepository : AbstractBaseRepository<Employer>
    {
        private readonly ILogger<EmployerRepository> _logger;
        private readonly MatchingDbContext _dbContext;

        public EmployerRepository(ILogger<EmployerRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public override async Task<int> CreateMany(IEnumerable<Employer> employers)
        {
            await ResetData();

            return await BaseCreateMany(entities);
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