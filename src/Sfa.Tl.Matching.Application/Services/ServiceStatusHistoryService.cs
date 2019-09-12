using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ServiceStatusHistoryService : IServiceStatusHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ServiceStatusHistory> _maintenanceHistoryRepository;

        public ServiceStatusHistoryService(IMapper mapper, IRepository<ServiceStatusHistory> maintenanceHistoryRepository)
        {
            _mapper = mapper;
            _maintenanceHistoryRepository = maintenanceHistoryRepository;
        }

        public async Task<ServiceStatusHistoryViewModel> GetLatestMaintenanceHistory()
        {
            var maintenanceHistory = await _maintenanceHistoryRepository.GetLastOrDefault(mh => true);
            if (maintenanceHistory == null)
                return new ServiceStatusHistoryViewModel();

            var viewModel = _mapper.Map<ServiceStatusHistory, ServiceStatusHistoryViewModel>(maintenanceHistory);

            return viewModel;
        }

        public async Task<int> SaveServiceStatusHistory(ServiceStatusHistoryViewModel viewModel)
        {
            viewModel.IsOnline = !viewModel.IsOnline;
            var maintenanceHistory = _mapper.Map<ServiceStatusHistoryViewModel, ServiceStatusHistory>(viewModel);
            
            return await _maintenanceHistoryRepository.Create(maintenanceHistory);
        }
    }
}