using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Employer> _repository;

        public EmployerService(
            IMapper mapper,
            IRepository<Employer> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<EmployerDto> GetEmployer(int id)
        {
            var employer = await _repository.GetSingleOrDefault(e => e.Id == id);

            var dto = _mapper.Map<Employer, EmployerDto>(employer);

            return dto;
        }

        public IEnumerable<EmployerSearchResultDto> Search(string employerName)
        {
            var searchResults = _repository
                .GetMany(e => 
                    EF.Functions.Like(e.CompanyName, $"%{employerName}%"));

            var employers = searchResults.Select(e => new EmployerSearchResultDto
            {
                Id = e.Id,
                EmployerName = e.CompanyName,
                AlsoKnownAs = e.AlsoKnownAs
            }).OrderBy(e => e.EmployerName);

            return employers;
        }
    }
}