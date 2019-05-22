using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISqlBulkRepository
    {
        Task BulkInsertIntoToStaging<T>(List<T> entities);
    }
}