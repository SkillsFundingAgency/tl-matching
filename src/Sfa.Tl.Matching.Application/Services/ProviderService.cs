using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IMapper _searchResultMapper;
        private readonly IFileReader<ProviderFileImportDto, ProviderDto> _fileReader;
        private readonly IRepository<Provider> _repository;
        private readonly ISearchProvider _searchProvider;

        public ProviderService(
            IMapper mapper,
            IFileReader<ProviderFileImportDto, ProviderDto> fileReader,
            IRepository<Provider> repository,
            IMapper searchResultMapper,
            ISearchProvider searchProvider)
        {
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
            _searchResultMapper = searchResultMapper;
            _searchProvider = searchProvider;
        }

        public async Task<int> ImportProvider(ProviderFileImportDto fileImportDto)
        {
            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            var createdRecords = 0;
            if (import != null && import.Any())
            {
                var providers = _mapper.Map<IEnumerable<Provider>>(import);
                createdRecords = await _repository.CreateMany(providers);
            }

            return createdRecords;
        }

        public void UpdateProvider()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId)
        {
            var searchResults = await _searchProvider.SearchProvidersByPostcodeProximity(postcode, searchRadius, routeId);

            var results = searchResults.Any()
                ? _searchResultMapper.Map<IEnumerable<ProviderVenueSearchResultDto>>(searchResults)
                : new List<ProviderVenueSearchResultDto>();

            return results;
        }
    }
}