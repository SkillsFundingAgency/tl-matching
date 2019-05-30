using System;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferenceDataService
    {
        Task<int> SynchronizeProviderReference(DateTime lastUpdateDate);
    }
}