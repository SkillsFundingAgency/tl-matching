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
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ILogger _logger;
        private readonly MatchingDbContext _dbContext;

        public GenericRepository(ILogger<GenericRepository<T>> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public virtual async Task<int> CreateMany(IList<T> entities)
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

        public virtual async Task UpdateMany(IList<T> entities)
        {
            _dbContext.UpdateRange(entities);

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

        public virtual async Task UpdateWithSpecifedColumnsOnly(T entity, params Expression<Func<T, object>>[] properties)
        {
            foreach (var property in properties)
                _dbContext.Entry(entity).Property(property).IsModified = true;

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

        public virtual async Task Delete(T entity)
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

        public virtual async Task<int> Delete(int id)
        {
            var entity = new T
            {
                Id = id
            };

            _dbContext.Attach(entity);
            _dbContext.Remove(entity);

            int deletedRecordCount;
            try
            {
                deletedRecordCount = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }

            return deletedRecordCount;
        }

        public virtual async Task DeleteMany(IList<T> entities)
        {
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

        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return predicate != null ? queryable.Where(predicate) : queryable;
        }

        public async Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return await queryable.SingleOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TDto> GetSingleOrDefault<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);

            return await queryable.Select(selector).SingleOrDefaultAsync();

        }

        public async Task<TDto> GetFirstOrDefault<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, asendingorder, navigationPropertyPath);

            return await queryable.Select(selector).FirstOrDefaultAsync();
        }

        private IQueryable<T> GetQueryableWithIncludes(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<T>().AsQueryable();

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