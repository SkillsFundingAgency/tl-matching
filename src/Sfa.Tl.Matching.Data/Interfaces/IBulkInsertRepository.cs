using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IBulkInsertRepository<T> where T : BaseEntity, new()
    {
        Task BulkInsertAsync(IList<T> entities);
        Task<int> MergeFromStagingAsync();
    }
}