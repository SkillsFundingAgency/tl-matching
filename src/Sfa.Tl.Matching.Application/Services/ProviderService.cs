using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
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
        private readonly IRepository<ProviderReference> _referenceRepository;

        public ProviderService(IMapper mapper, IRepository<Provider> repository,
            IRepository<ProviderReference> referenceRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _referenceRepository = referenceRepository;
        }

        public async Task<IList<ProviderSearchResultItemViewModel>> SearchProvidersWithFundingAsync(ProviderSearchParametersViewModel searchParameters)
        {
            return await _repository.GetMany(p => searchParameters.UkPrn == null || p.UkPrn == searchParameters.UkPrn.Value)
                                    .OrderBy(p => p.Name)
                                    .ProjectTo<ProviderSearchResultItemViewModel>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
        }

        public async Task<int> GetProvidersWithFundingCountAsync()
        {
            return await _repository
                .GetMany(p => p.IsCdfProvider)
                .CountAsync();
        }

        public async Task<ProviderSearchResultDto> SearchAsync(long ukPrn)
        {
            var provider = await _repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);
            var dto = _mapper.Map<Provider, ProviderSearchResultDto>(provider);

            return dto;
        }

        public async Task<ProviderSearchResultDto> SearchReferenceDataAsync(long ukPrn)
        {
            var providerReference = await _referenceRepository.GetSingleOrDefault(pr => pr.UkPrn == ukPrn);
            return _mapper.Map<ProviderReference, ProviderSearchResultDto>(providerReference);
        }

        public async Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId)
        {
            var provider = await _repository
                .GetMany(p => p.Id == providerId)
                .Select(p => new ProviderDetailViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    UkPrn = p.UkPrn,
                    DisplayName = p.DisplayName,
                    PrimaryContact = p.PrimaryContact,
                    PrimaryContactPhone = p.PrimaryContactPhone,
                    PrimaryContactEmail = p.PrimaryContactEmail,
                    SecondaryContact = p.SecondaryContact,
                    SecondaryContactPhone = p.SecondaryContactPhone,
                    SecondaryContactEmail = p.SecondaryContactEmail,
                    IsCdfProvider = p.IsCdfProvider,
                    IsEnabledForReferral = p.IsEnabledForReferral,
                    IsTLevelProvider = p.IsTLevelProvider,
                    Source = p.Source,
                    ProviderVenues = p.ProviderVenue
                    .Where(venue => !venue.IsRemoved)
                    .Select(venue => new ProviderVenueViewModel
                    {
                        ProviderVenueId = venue.Id,
                        Postcode = venue.Postcode,
                        IsRemoved = venue.IsRemoved,
                        IsEnabledForReferral = venue.IsEnabledForReferral,
                        QualificationCount = venue.ProviderQualification.Count
                    })
                    .OrderBy(v => v.Postcode).ToList()
                })
                .SingleOrDefaultAsync();

            return provider;
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
            var trackedEntity = await _repository.GetSingleOrDefault(p => p.Id == viewModel.Id);
            trackedEntity = _mapper.Map(viewModel, trackedEntity);

            await _repository.Update(trackedEntity);
        }

        public async Task<int> CreateProvider(CreateProviderDetailViewModel viewModel)
        {
            var provider = _mapper.Map<CreateProviderDetailViewModel, Provider>(viewModel);

            return await _repository.Create(provider);
        }
    }
}