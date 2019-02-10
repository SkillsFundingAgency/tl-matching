using System;
using System.Collections.Generic;
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
        private readonly IFileReader<ProviderFileImportDto, ProviderDto> _fileReader;
        private readonly IRepository<Provider> _repository;

        public ProviderService(
            IMapper mapper,
            IFileReader<ProviderFileImportDto, ProviderDto> fileReader,
            IRepository<Provider> repository)
        {
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
        }

        public async Task<int> ImportProvider(ProviderFileImportDto fileImportDto)
        {
            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            var createdRecords = 0;
            if (import != null)
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

        public void SearchProviderByPostCodeProximity()
        {
            throw new NotImplementedException();
        }
    }
}