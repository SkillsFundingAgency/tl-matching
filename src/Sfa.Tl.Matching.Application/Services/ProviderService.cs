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
                .GetMany(p => p.IsCdfProvider)
                .CountAsync();

            return await query;
        }

        public async Task<ProviderSearchResultDto> SearchAsync(long ukPrn)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            var dto = _mapper.Map<Provider, ProviderSearchResultDto>(provider);

            return dto;
        }

        public async Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId)
        {
            var provider = await _repository
                .GetMany(p => p.Id == providerId)
                .Include(p => p.ProviderVenue)
                .ThenInclude(pv => pv.ProviderQualification)
                .SingleOrDefaultAsync();

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
                    IsRemoved = pv.IsRemoved,
                    QualificationCount = pv.ProviderQualification.Count,
                }).ToListAsync();
        }

        public async Task DeleteProviderAsync(int id)
        {
            await _repository.Delete(id);
        }

        public async Task UpdateProviderDetailSectionAsync(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            await _repository.UpdateWithSpecifedColumnsOnly(provider,
                x => x.IsCdfProvider,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        public async Task UpdateProviderDetail(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            await _repository.Update(provider);
        }

        public async Task<int> CreateProvider(ProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<ProviderDetailViewModel, Provider>(viewModel);

            return await _repository.Create(provider);
        }

        public async Task<HideProviderViewModel> GetHideProviderViewModelAsync(int providerId)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.Id == providerId);

            return _mapper.Map<Provider, HideProviderViewModel>(provider);
        }

        public async Task UpdateProviderAsync(HideProviderViewModel viewModel)
        {
            var provider = _mapper.Map<HideProviderViewModel, Provider>(viewModel);

            await _repository.UpdateWithSpecifedColumnsOnly(provider,
                x => x.IsCdfProvider,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private static List<ProviderSearchResultItemViewModel> GetProvidersToUpdate(IEnumerable<ProviderSearchResultItemViewModel> providersFromVm,
            IEnumerable<ProviderSearchResultItemViewModel> providersFromDb)
        {
            var providersToUpdate = (from pDb in providersFromDb
                                     join pVm in providersFromVm on pDb.ProviderId equals pVm.ProviderId
                                     where pDb.ProviderId == pVm.ProviderId
                                           && pDb.IsCdfProvider != pVm.IsCdfProvider
                                     select new ProviderSearchResultItemViewModel
                                     {
                                         IsCdfProvider = pVm.IsCdfProvider,
                                         ProviderId = pVm.ProviderId
                                     }).ToList();

            return providersToUpdate;
        }
    }
}