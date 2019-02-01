using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IEmployerCommandRepository
    {
        Task<int> CreateMany(List<Employer> employers);
        Task ResetData();
    }
}