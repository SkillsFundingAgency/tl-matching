using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity, new()
    {
        Task<int> CreateAsync(T entity);
        Task<int> CreateManyAsync(IList<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateManyAsync(IList<T> entities);
        Task UpdateWithSpecifiedColumnsOnlyAsync(T entity, params Expression<Func<T, object>>[] properties);
        Task UpdateManyWithSpecifiedColumnsOnlyAsync(IList<T> entities, params Expression<Func<T, object>>[] properties);

        Task BulkUpdateManyAsync(IList<T> entities);

        Task BulkUpdateManyWithSpecifiedColumnsOnlyAsync(IList<T> entities, params Expression<Func<T, object>>[] properties);

        Task<int> DeleteAsync(int id);
        Task DeleteAsync(T entity);
        Task DeleteManyAsync(IList<T> entities);

        IQueryable<T> GetManyAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<TDto> GetFirstOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy = null, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath);

        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<TDto> GetSingleOrDefaultAsync<TDto>(Expression<Func<T, bool>> predicate, Expression<Func<T, TDto>> selector, Expression<Func<T, object>> orderBy = null, bool asendingorder = true, params Expression<Func<T, object>>[] navigationPropertyPath);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
    }
}