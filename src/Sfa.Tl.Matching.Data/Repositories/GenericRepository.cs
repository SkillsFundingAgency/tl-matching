using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly ILogger _logger;

        // ReSharper disable InconsistentNaming
        protected readonly MatchingDbContext _dbContext;

        public GenericRepository(ILogger<GenericRepository<T>> logger, MatchingDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public virtual async Task<int> CreateManyAsync(IList<T> entities)
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

        public virtual async Task<int> CreateAsync(T entity)
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

        public virtual async Task UpdateAsync(T entity)
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

        public virtual async Task UpdateManyAsync(IList<T> entities)
        {
            if (entities.Count == 0) return;

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

        public virtual async Task BulkUpdateManyAsync(IList<T> entities)
        {
            if (entities.Count == 0) return;

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.Execute(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await _dbContext.BulkUpdateAsync(entities,
                        config => config.UseTempDB = true);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex.InnerException);
                    await transaction.RollbackAsync();
                    throw;
                }
            });
        }

        public virtual async Task UpdateWithSpecifiedColumnsOnlyAsync(T entity, params Expression<Func<T, object>>[] properties)
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

        public virtual async Task UpdateManyWithSpecifiedColumnsOnlyAsync(IList<T> entities,
            params Expression<Func<T, object>>[] properties)
        {
            foreach (var entity in entities)
            {
                foreach (var property in properties)
                {
                    _dbContext.Entry(entity).Property(property).IsModified = true;
                }
            }

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

        public virtual async Task BulkUpdateManyWithSpecifiedColumnsOnlyAsync(IList<T> entities, params Expression<Func<T, object>>[] properties)
        {
            try
            {
                var propList = properties.Select(pro =>
                {
                    return pro.Body switch
                    {
                        UnaryExpression expression => ((MemberExpression) expression.Operand).Member.Name,
                        MemberExpression expression1 => expression1.Member.Name,
                        _ => throw new InvalidOperationException("unable to extract PropertyName for Bulk Update")
                    };
                }).ToList();

                var strategy = _dbContext.Database.CreateExecutionStrategy();

                await strategy.Execute(async () =>
                {
                    await using var transaction = await _dbContext.Database.BeginTransactionAsync();
                    try
                    {
                        await _dbContext.BulkUpdateAsync(entities,
                            config =>
                            {
                                config.PropertiesToInclude = propList;
                                config.UseTempDB = true;
                            });
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex.InnerException);
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (DbUpdateException due)
            {
                _logger.LogError(due.Message, due.InnerException);
                throw;
            }
        }

        public virtual async Task DeleteAsync(T entity)
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

        public virtual async Task<int> DeleteAsync(int id)
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

        public virtual async Task DeleteManyAsync(IList<T> entities)
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

        public IQueryable<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return predicate != null ? queryable.Where(predicate) : queryable;
        }

        public async Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return await queryable.SingleOrDefaultAsync();
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, null, true, navigationPropertyPath);

            return await queryable.FirstOrDefaultAsync();
        }

        public async Task<TDto> GetSingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool ascendingOrder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, ascendingOrder, navigationPropertyPath);

            return await queryable.Select(selector).SingleOrDefaultAsync();
        }

        public async Task<TDto> GetFirstOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy, bool ascendingOrder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = GetQueryableWithIncludes(predicate, orderBy, ascendingOrder, navigationPropertyPath);

            return await queryable.Select(selector).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? await _dbContext.Set<T>().CountAsync(predicate) :
                await _dbContext.Set<T>().CountAsync();
        }

        private IQueryable<T> GetQueryableWithIncludes(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> orderBy, bool ascendingOrder = true, params Expression<Func<T, object>>[] navigationPropertyPath)
        {
            var queryable = _dbContext.Set<T>().AsQueryable();

            if (navigationPropertyPath != null && navigationPropertyPath.Any())
            {
                queryable = navigationPropertyPath.Aggregate(queryable, (current, navProp) => current.Include(navProp));
            }

            if (predicate != null)
                queryable = queryable.Where(predicate);

            if (orderBy != null)
                queryable = ascendingOrder ? queryable.OrderBy(orderBy) : queryable.OrderByDescending(orderBy);

            return queryable;
        }
    }
}