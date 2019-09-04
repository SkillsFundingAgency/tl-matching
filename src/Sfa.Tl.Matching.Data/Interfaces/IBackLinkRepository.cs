using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IBackLinkRepository
    {
        Task<int> Create(BackLinkHistory entity);
        Task DeleteMany(IList<BackLinkHistory> entities);
        Task Delete(BackLinkHistory entity);
        IQueryable<BackLinkHistory> GetMany(Expression<Func<BackLinkHistory, bool>> predicate = null,
            params Expression<Func<BackLinkHistory, object>>[] navigationPropertyPath);
        Task<BackLinkHistory> GetLastOrDefault(Expression<Func<BackLinkHistory, bool>> predicate,
            params Expression<Func<BackLinkHistory, object>>[] navigationPropertyPath);
    }
}
