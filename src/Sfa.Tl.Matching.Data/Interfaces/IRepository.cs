using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<int> CreateMany(IEnumerable<T> entities);
        Task<T> GetSingleOrDefault(Func<T, bool> predicate);
    }
}