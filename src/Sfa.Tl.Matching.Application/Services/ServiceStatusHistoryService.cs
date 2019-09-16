using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ServiceStatusHistoryService : IServiceStatusHistoryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ServiceStatusHistory> _serviceStatusHistoryRepository;

        public ServiceStatusHistoryService(IMapper mapper, IRepository<ServiceStatusHistory> serviceStatusHistoryRepository)
        {
            _mapper = mapper;
            _serviceStatusHistoryRepository = serviceStatusHistoryRepository;
        }

        public async Task<ServiceStatusHistoryViewModel> GetLatestServiceStatusHistory()
        {
            var serviceStatusHistory = await _serviceStatusHistoryRepository.GetMany(ssh => true)
                .OrderByDescending(ssh => ssh.Id)
                .FirstOrDefaultAsync();

            if (serviceStatusHistory == null)
                return new ServiceStatusHistoryViewModel();

            var viewModel = _mapper.Map<ServiceStatusHistory, ServiceStatusHistoryViewModel>(serviceStatusHistory);

            return viewModel;
        }

        public async Task<int> SaveServiceStatusHistory(ServiceStatusHistoryViewModel viewModel)
        {
            viewModel.IsOnline = !viewModel.IsOnline;
            var serviceStatusHistory = _mapper.Map<ServiceStatusHistoryViewModel, ServiceStatusHistory>(viewModel);
            
            return await _serviceStatusHistoryRepository.Create(serviceStatusHistory);
        }
    }
}