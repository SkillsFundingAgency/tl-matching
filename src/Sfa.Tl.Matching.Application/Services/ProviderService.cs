using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _mapper;
        private readonly IDataImportService<ProviderDto> _dataImportService;
        private readonly IRepository<Provider> _repository;

        public ProviderService(
            IMapper mapper,
            IDataImportService<ProviderDto> dataImportService,
            IRepository<Provider> repository)
        {
            _mapper = mapper;
            _dataImportService = dataImportService;
            _repository = repository;
        }

        public async Task<int> ImportProvider(Stream stream)
        {
            var import = _dataImportService.Import(stream, DataImportType.Provider);

            int createdRecords = 0;
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