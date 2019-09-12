using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IServiceStatusHistoryService
    {
        Task<ServiceStatusHistoryViewModel> GetLatestServiceStatusHistory();
        Task<int> SaveServiceStatusHistory(ServiceStatusHistoryViewModel viewModel);
    }
}