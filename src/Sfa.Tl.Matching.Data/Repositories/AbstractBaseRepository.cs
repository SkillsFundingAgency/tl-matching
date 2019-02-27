using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public abstract class AbstractBaseRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ILogger _logger;
        private readonly MatchingDbContext _dbContext;

        protected AbstractBaseRepository(ILogger logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> BaseCreateMany(IEnumerable<T> entities)
        {
            await _dbContext.AddRangeAsync(entities);

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

        public virtual async Task<int> Create(T entity)
        {
            await _dbContext.AddAsync(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return entity.Id;
        }

        public virtual async Task Update(T entity)
        {
            _dbContext.Update(entity);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public abstract Task<int> CreateMany(IList<T> entities);

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var dbSet = GetSetWithIncludes(navigationPropertyPath);

            return predicate != null ? dbSet.Where(predicate) : dbSet;
        }

        public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var dbSet = GetSetWithIncludes(navigationPropertyPath);

            return await dbSet.SingleOrDefaultAsync(predicate);
        }

        private IQueryable<T> GetSetWithIncludes(Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var dbSet = _dbContext.Set<T>().AsQueryable();

            if (navigationPropertyPath.Any())
            {
                foreach (var navProp in navigationPropertyPath)
                {
                    dbSet.Include(navProp);
                }
            }

            return dbSet;
        }
    }
}