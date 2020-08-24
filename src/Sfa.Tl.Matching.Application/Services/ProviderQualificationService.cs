using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
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

        public async Task<ProviderQualificationDto> GetProviderQualificationAsync(int providerVenueId, int qualificationId)
        {
            var providerQualification = await _providerQualificationRepository
                .GetSingleOrDefaultAsync(p => p.ProviderVenueId == providerVenueId && 
                                              p.QualificationId == qualificationId);

            return providerQualification != null && !providerQualification.IsDeleted 
                ? _mapper.Map<ProviderQualificationDto>(providerQualification)
                : null;
        }
        
        public async Task<int> CreateProviderQualificationAsync(AddQualificationViewModel viewModel)
        {
            var providerQualification = await _providerQualificationRepository.GetSingleOrDefaultAsync(
                pq => pq.ProviderVenueId == viewModel.ProviderVenueId
                      && pq.QualificationId == viewModel.QualificationId);

            if (providerQualification != null)
            {
                return providerQualification.Id;
            }

            providerQualification = _mapper.Map<ProviderQualification>(viewModel);
            return await _providerQualificationRepository.CreateAsync(providerQualification);
        }

        public async Task RemoveProviderQualificationAsync(int providerVenueId, int qualificationId)
        {
            var providerQualifications = await _providerQualificationRepository.GetManyAsync(
                pq => pq.ProviderVenueId == providerVenueId
                      && pq.QualificationId == qualificationId).ToListAsync();

            if (providerQualifications != null)
            {
                await _providerQualificationRepository.DeleteManyAsync(providerQualifications);
            }
        }
    }
}
