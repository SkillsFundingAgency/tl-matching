using System.Collections.Generic;
using System.Linq;
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

        public async Task<IList<ProviderSearchResultItemViewModel>> SearchProvidersWithFundingAsync(ProviderSearchParametersViewModel searchParameters)
        {
            var query = _repository.GetMany();

            if (searchParameters.UkPrn.HasValue)
                query = query.Where(p => p.UkPrn == searchParameters.UkPrn.Value);
                
            query = query.OrderBy(p => p.Name);

            var providers = await query.ToListAsync();
            return _mapper.Map<IList<Provider>, IList<ProviderSearchResultItemViewModel>>(providers);

            //return await query.ProjectTo<ProviderSearchResultItemViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }
        
        public async Task<int> GetProvidersWithFundingCountAsync()
        {
            var query = _repository
                .GetMany(p => p.IsFundedForNextYear)
                .CountAsync();

            return await query;
        }

        public async Task<ProviderSearchResultDto> SearchAsync(long ukPrn)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            var dto = _mapper.Map<Provider, ProviderSearchResultDto>(provider);

            return dto;
        }

        public async Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId, bool includeVenueDetails = false)
        {
            var query = _repository.GetMany(p => p.Id == providerId);

            if (includeVenueDetails)
            {
                query = query.Include(p => p.ProviderVenue).ThenInclude(pv => pv.ProviderQualification);
            }

            var provider = await query.SingleOrDefaultAsync();

            return _mapper.Map<Provider, ProviderDetailViewModel>(provider);
        }

        public async Task<IList<ProviderVenueViewModel>> GetProviderVenueSummaryByProviderIdAsync(int providerId, bool includeVenueDetails = false)
        {
            return await _repository.GetMany(p => p.Id == providerId)
                .SelectMany(p => p.ProviderVenue)
                .Select(pv => new ProviderVenueViewModel
                {
                    ProviderVenueId = pv.Id,
                    Postcode = pv.Postcode,
                    IsEnabledForSearch = pv.IsEnabledForSearch,
                    QualificationCount = pv.ProviderQualification.Count,
                }).ToListAsync();
        }

        public Task UpdateProviderDetail(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            return _repository.Update(provider);
        }

        public Task<int> CreateProvider(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            return _repository.Create(provider);
        }

        public async Task<HideProviderViewModel> GetHideProviderViewModelAsync(int providerId)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.Id == providerId);

            return _mapper.Map<Provider, HideProviderViewModel>(provider);
        }

        public async Task SetIsProviderEnabledForSearchAsync(int providerId, bool isEnabled)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.Id == providerId);

            if (provider != null)
            {
                provider.IsEnabledForSearch = isEnabled;
                await _repository.Update(provider);
            }
        }

        public Task UpdateProvider(SaveProviderFeedbackViewModel viewModel)
        {
            var providers = _mapper.Map<IList<Provider>>(viewModel.Providers);

            return _repository.UpdateManyWithSpecifedColumnsOnly(providers, 
                x => x.IsFundedForNextYear);//,
        }
    }
}