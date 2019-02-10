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
    public class EmployerService : IEmployerService
    {
        private readonly IMapper _mapper;
        private readonly IFileReader<EmployerFileImportDto, EmployerDto> _fileReader;
        private readonly IRepository<Employer> _repository;

        public EmployerService(
            IMapper mapper,
            IFileReader<EmployerFileImportDto, EmployerDto> fileReader,
            IRepository<Employer> repository)
        {
            _mapper = mapper;
            _repository = repository;
            _fileReader = fileReader;
        }

        public async Task<int> ImportEmployer(EmployerFileImportDto fileImportDto)
        {
            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            if (import != null)
            {
                var employers = _mapper.Map<IEnumerable<Employer>>(import);
                return await _repository.CreateMany(employers);
            }

            return 0;
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