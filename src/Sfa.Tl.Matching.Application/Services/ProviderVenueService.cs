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
    public class ProviderVenueService : IProviderVenueService
    {
        private readonly IMapper _mapper;
        private readonly IFileReader<ProviderVenueFileImportDto, ProviderVenueDto> _fileReader;
        private readonly IRepository<ProviderVenue> _repository;

        public ProviderVenueService(
            IMapper mapper,
            IFileReader<ProviderVenueFileImportDto, ProviderVenueDto> fileReader,
            IRepository<ProviderVenue> repository)
        {
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
        }

        public async Task<int> ImportProviderVenue(ProviderVenueFileImportDto fileImportDto)
        {
            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            if (import != null && import.Any())
            {
                var providerVenues = _mapper.Map<IEnumerable<ProviderVenue>>(import);
                return await _repository.CreateMany(providerVenues);
            }

            return 0;
        }

        public void CreateProviderVenue()
        {
            throw new NotImplementedException();
        }

        public void UpdateProviderVenue()
        {
            throw new NotImplementedException();
        }
    }
}