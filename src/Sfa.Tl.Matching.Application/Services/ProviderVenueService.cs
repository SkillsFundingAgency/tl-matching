using System;
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