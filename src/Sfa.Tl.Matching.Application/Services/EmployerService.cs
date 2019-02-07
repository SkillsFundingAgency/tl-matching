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
    public class EmployerService : IEmployerService
    {
        private readonly IMapper _mapper;
        private readonly IDataImportService<EmployerDto> _dataImportService;
        private readonly IRepository<Employer> _repository;

        public EmployerService(
            IMapper mapper,
            IDataImportService<EmployerDto> dataImportService,
            IRepository<Employer> repository)
        {
            _mapper = mapper;
            _dataImportService = dataImportService;
            _repository = repository;
        }

        public async Task ImportEmployer(Stream dataStream)
        {
            var import = _dataImportService.Import(dataStream, DataImportType.Employer, 1);

            if (import != null)
            {
                var employers = _mapper.Map<IEnumerable<Employer>>(import);
               await _repository.CreateMany(employers);
            }
        }

        public void GetEmployerByName()
        {
            throw new NotImplementedException();
        }

        public void CreateEmployer()
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployer()
        {
            throw new NotImplementedException();
        }
    }
}