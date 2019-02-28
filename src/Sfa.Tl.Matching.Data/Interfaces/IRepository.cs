﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        Task<int> Create(T entity);
        Task<int> CreateMany(IList<T> entities);
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] navigationPropertyPath);

        Task<T> GetSingleOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] navigationPropertyPath);
        Task Update(T entity);
    }
}