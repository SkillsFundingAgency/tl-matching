using System.Collections.Generic;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class NullDataProcessor<TEntity> : IDataProcessor<TEntity> where TEntity : BaseEntity
    {
        public void PreProcessingHandler(IList<TEntity> entities) { }

        public void PostProcessingHandler(IList<TEntity> entities) { }
    }
}