using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataProcessor<TEntity> where TEntity : BaseEntity
    {
        void PreProcessingHandler(IList<TEntity> entities);
        void PostProcessingHandler(IList<TEntity> entities);
    }
}