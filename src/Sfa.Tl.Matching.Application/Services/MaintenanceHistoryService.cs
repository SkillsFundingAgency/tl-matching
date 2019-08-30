using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class MaintenanceHistoryService : IMaintenanceHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<MaintenanceHistory> _maintenanceHistoryRepository;

        public MaintenanceHistoryService(IMapper mapper, IRepository<MaintenanceHistory> maintenanceHistoryRepository)
        {
            _mapper = mapper;
            _maintenanceHistoryRepository = maintenanceHistoryRepository;
        }

        public async Task<MaintenanceViewModel> GetLatestMaintenanceHistory()
        {
            var maintenanceHistory = await _maintenanceHistoryRepository.GetLastOrDefault(mh => true);
            var viewModel = _mapper.Map<MaintenanceHistory, MaintenanceViewModel>(maintenanceHistory);

            return viewModel;
        }

        public async Task<int> SaveMaintenanceHistory(MaintenanceViewModel viewModel)
        {
            viewModel.IsOnline = !viewModel.IsOnline;
            var maintenanceHistory = _mapper.Map<MaintenanceViewModel, MaintenanceHistory>(viewModel);
            
            return await _maintenanceHistoryRepository.Create(maintenanceHistory);
        }
    }
}