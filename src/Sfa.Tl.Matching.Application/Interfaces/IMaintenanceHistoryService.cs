using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IMaintenanceHistoryService
    {
        Task<MaintenanceViewModel> GetLatestMaintenanceHistory();
        Task<int> SaveMaintenanceHistory(MaintenanceViewModel viewModel);
    }
}