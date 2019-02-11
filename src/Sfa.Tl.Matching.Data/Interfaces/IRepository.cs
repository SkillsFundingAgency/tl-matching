using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<int> CreateMany(IEnumerable<T> entities);

        Task<IQueryable<T>> GetMany(Func<T, bool> predicate);

        Task<T> GetSingleOrDefault(Func<T, bool> predicate);
    }
}