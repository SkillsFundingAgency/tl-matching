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

            if (import != null && import.Any())
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

        public async Task<EmployerDto> GetEmployer(string companyName, string alsoKnownAs)
        {
            var employer = await _repository.GetSingleOrDefault(e => e.CompanyName == companyName &&
                                                e.AlsoKnownAs == alsoKnownAs);

            var dto = _mapper.Map<Employer, EmployerDto>(employer);

            return dto;
        }

        public void CreateEmployer()
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployer()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EmployerSearchResultDto>> Search(string employerName)
        {
            var searchResults = await _repository.GetMany(e => e.CompanyName.Contains(employerName, StringComparison.CurrentCultureIgnoreCase));

            var employers = searchResults.Select(e => new EmployerSearchResultDto
            {
                EmployerName = e.CompanyName,
                AlsoKnownAs = e.AlsoKnownAs
            }).OrderBy(e => e.EmployerName);

            return employers;
        }
    }
}