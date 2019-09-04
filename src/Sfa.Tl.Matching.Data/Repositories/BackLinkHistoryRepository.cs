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
    public class BackLinkHistoryRepository : IBackLinkRepository
    {
        private readonly ILogger<BackLinkHistoryRepository> _logger;
        private readonly MatchingDbContext _dbContext;
        public BackLinkHistoryRepository(ILogger<BackLinkHistoryRepository> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> Create(BackLinkHistory entity)
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

        public async Task Delete(BackLinkHistory entity)
        {
            _dbContext.Remove(entity);

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

        public async Task DeleteMany(IList<BackLinkHistory> entities)
        {
            if (entities.Count == 0) return;

            _dbContext.RemoveRange(entities);

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

        public IQueryable<BackLinkHistory> GetMany(Expression<Func<BackLinkHistory, bool>> predicate = null, params Expression<Func<BackLinkHistory, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return predicate != null ? queryable.Where(predicate) : queryable;
        }

        public async Task<BackLinkHistory> GetLastOrDefault(Expression<Func<BackLinkHistory, bool>> predicate, params Expression<Func<BackLinkHistory, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return await queryable.LastOrDefaultAsync();
        }

        private IQueryable<BackLinkHistory> GetQueryableWithIncludes(Expression<Func<BackLinkHistory, bool>> predicate, Expression<Func<BackLinkHistory, object>> orderBy, bool asendingorder = true, params Expression<Func<BackLinkHistory, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<BackLinkHistory>().AsQueryable();

            if (navigationPropertyPath != null && navigationPropertyPath.Any())
            {
                queryable = navigationPropertyPath.Aggregate(queryable, (current, navProp) => current.Include(navProp));
            }

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = asendingorder ? queryable.OrderBy(orderBy) : queryable.OrderByDescending(orderBy);

            return queryable;
        }
    }
}
