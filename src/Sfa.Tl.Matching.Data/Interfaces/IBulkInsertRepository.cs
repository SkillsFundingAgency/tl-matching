using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IBulkInsertRepository<T> where T : BaseEntity, new()
    {
        Task BulkInsertAsync(IEnumerable<T> entities);
        Task<int> MergeFromStagingAsync();
    }
}