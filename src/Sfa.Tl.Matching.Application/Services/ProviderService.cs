using Sfa.Tl.Matching.Application.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Provider> _repository;

        public ProviderService(IMapper mapper, IRepository<Provider> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ProviderSearchResultDto> SearchAsync(long ukPrn)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            var dto = _mapper.Map<Provider, ProviderSearchResultDto>(provider);

            return dto;
        }

        public Task UpdateProvider(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            return _repository.Update(provider);
        }

        public Task<int> CreateProvider(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            return _repository.Create(provider);
        }

        public async Task<ProviderDetailViewModel> GetProviderDetailByUkprnAsync(long ukPrn, bool includeVeuneDetails = false)
        {
            var query = _repository.GetMany(p => p.UkPrn == ukPrn);

            if (includeVeuneDetails)
            {
                query.Include(p => p.ProviderVenue).ThenInclude(pv => pv.ProviderQualification);
            }

            var provider = await query.SingleOrDefaultAsync();

            return _mapper.Map<Provider, ProviderDetailViewModel>(provider);
        }
        
        public async Task<ProviderDto> GetProviderByUkPrnAsync(long ukPrn)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            var dto = provider != null
                ? _mapper.Map<Provider, ProviderDto>(provider)
                : null;

            return dto;
        }

        public async Task SetIsProviderEnabledAsync(int providerId, bool isEnabled)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.Id == providerId);
            if (provider != null)
            {
                provider.IsEnabledForSearch = isEnabled;
                await _repository.Update(provider);
            }
        }
    }
}