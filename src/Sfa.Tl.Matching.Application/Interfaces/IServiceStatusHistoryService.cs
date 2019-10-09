using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IServiceStatusHistoryService
    {
        Task<ServiceStatusHistoryViewModel> GetLatestServiceStatusHistoryAsync();
        Task<int> SaveServiceStatusHistoryAsync(ServiceStatusHistoryViewModel viewModel);
    }
}