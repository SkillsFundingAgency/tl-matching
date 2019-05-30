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
        Task<int> Create(T entity);
        Task<int> CreateMany(IList<T> entities);
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task Update(T entity);
        Task UpdateMany(IList<T> entities);
        Task UpdateWithSpecifedColumnsOnly(T entity, params Expression<Func<T, object>>[] properties);
        Task<int> Delete(int id);
        Task Delete(T entity);
        Task DeleteMany(IList<T> entities);
        Task BulkInsert(List<T> entities);
        Task<int> MergeFromStaging();
    }
}