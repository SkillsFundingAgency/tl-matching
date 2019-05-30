using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderQualificationService : IProviderQualificationService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ProviderQualification> _providerQualificationRepository;

        public ProviderQualificationService(IMapper mapper,
            IRepository<ProviderQualification> providerQualificationRepository)
        {
            _mapper = mapper;
            _providerQualificationRepository = providerQualificationRepository;
        }

        public async Task<int> CreateProviderQualificationAsync(AddQualificationViewModel viewModel)
        {
            var providerQualification = await _providerQualificationRepository.GetSingleOrDefault(
                pq => pq.ProviderVenueId == viewModel.ProviderVenueId
                      && pq.QualificationId == viewModel.QualificationId);

            if (providerQualification != null)
            {
                return providerQualification.Id;
            }

            providerQualification = _mapper.Map<ProviderQualification>(viewModel);
            return await _providerQualificationRepository.Create(providerQualification);
        }
    }
}
